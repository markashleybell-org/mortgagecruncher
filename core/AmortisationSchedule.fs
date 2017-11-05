namespace core

open System

type ScheduleType = FixedTerm | FixedPayments

type InterestRateType = Fixed | Variable

type InterestRate = { 
    Type:InterestRateType
    APR:float
    MonthlyRate:float }

type AmortisationScheduleEntry = { 
    PaymentDate:DateTime
    PaymentNumber:int
    InterestRate:InterestRate
    Payment:float
    OverPayment:float
    Principal:float
    Interest:float
    Balance:float
    LoanValue:float
    TermMonths:int }

module AmortisationSchedule =
    open Helpers
    open Excel.FinancialFunctions
    open System.Collections.Generic

    let fv = 0.00
    let typ = PaymentDue.EndOfPeriod

    // Schedule creation functions
    let private overPaymentAmount overPayments period = 
        match overPayments |> Map.tryFind period with
        | Some op -> op
        | None -> 0.00
    
    let private scheduleEntryData numberOfPayments overPayments monthlyInterestRate period = 
        let rate = monthlyInterestRate period
        let overPayment = overPaymentAmount overPayments period
        (period, ((numberOfPayments - period) + 1.00), rate, overPayment)

    let private calculatePaymentValuesFixedTerm interestRate numPeriods principal overPayment period =
        let principal' = principal + overPayment
        let pmt = Financial.Pmt(interestRate.MonthlyRate, numPeriods, principal', fv, typ)
        let ipmt = Financial.IPmt(interestRate.MonthlyRate, period, numPeriods, principal', fv, typ)
        let ppmt = Financial.PPmt(interestRate.MonthlyRate, period, numPeriods, principal', fv, typ)
        (pmt + overPayment, ipmt, ppmt + overPayment)

    let private scheduleEntryFixedTerm balance scheduleEntryData =
        let (period, numPeriods, interestRate, overPayment) = scheduleEntryData
        let (pmt, ipmt, ppmt) = calculatePaymentValuesFixedTerm interestRate numPeriods balance overPayment 1.00
        let newBalance = balance + ppmt
        ((int period, interestRate, pmt, ipmt, ppmt, newBalance), newBalance)

    let private calculatePaymentValuesFixedPayment interestRate principal overPayment paymentAmount =
        let principal' = principal + overPayment
        let pmt = paymentAmount
        let ipmt = (principal' * interestRate.MonthlyRate) * -1.00
        let ppmt = paymentAmount - ipmt
        (pmt + overPayment, ipmt, ppmt + overPayment)

    let private scheduleEntryFixedPayment paymentAmount balance schedulePeriod =
        let (period, _, interestRate, overPayment) = schedulePeriod
        let (pmt, ipmt, ppmt) = calculatePaymentValuesFixedPayment interestRate balance overPayment paymentAmount
        let newBalance = balance + ppmt
    
        let (pmt', ppmt', newBalance') = match newBalance < 0.00 with
                                         | true -> let payment = balance * -1.00
                                                   (payment, (payment + ipmt), (match newBalance > 0.00 with
                                                                                | true -> newBalance
                                                                                | false -> 0.00))
                                         | false -> (pmt, ppmt, newBalance)

        ((int period, interestRate, pmt', ipmt, ppmt', newBalance'), newBalance)


    let create term principal scheduleType variableRate fixedTerm fixedRate (overPayments:IDictionary<float, float>) = 
        let numberOfPayments = term * 12.00
        let numberOfFixedPayments = fixedTerm * 12.00
        let numberOfVariablePayments = numberOfPayments - numberOfFixedPayments

        let fixedRateMonthlyInterest = (fixedRate / 100.00) / 12.00
        let variableRateMonthlyInterest = (variableRate / 100.00) / 12.00

        let overPayments' = overPayments |> toMap;

        let monthlyInterestRate period =
            match period <= numberOfFixedPayments with
            | true -> { Type = Fixed; APR = fixedRate; MonthlyRate = fixedRateMonthlyInterest }
            | false -> { Type = Variable; APR = variableRate; MonthlyRate = variableRateMonthlyInterest }

        let createSchedulePeriodData = 
            scheduleEntryData numberOfPayments overPayments' monthlyInterestRate

        let schedulePeriods = [1.00..numberOfPayments] 
                              |> List.map createSchedulePeriodData

        match scheduleType with
        | ScheduleType.FixedTerm -> 
            schedulePeriods 
            |> List.mapFold scheduleEntryFixedTerm principal |> fst |> Array.ofList
        | ScheduleType.FixedPayments -> 
            let fixedRatePeriodPayment = 
                (Financial.Pmt(fixedRateMonthlyInterest, numberOfPayments, principal, fv, typ))

            let balanceAtEndOfFixedPeriod = 
                (Financial.Fv(fixedRateMonthlyInterest, numberOfFixedPayments, fixedRatePeriodPayment, principal, typ)) * -1.00

            let variableRatePeriodPayment = 
                (Financial.Pmt(variableRateMonthlyInterest, numberOfVariablePayments, balanceAtEndOfFixedPeriod, fv, typ))

            let schedule1 = schedulePeriods 
                            |> List.take (int numberOfFixedPayments)
                            |> List.mapFold (scheduleEntryFixedPayment fixedRatePeriodPayment) principal
                            |> fst

            let schedule2 = schedulePeriods 
                            |> List.skip (int numberOfFixedPayments)
                            |> List.mapFold (scheduleEntryFixedPayment variableRatePeriodPayment) balanceAtEndOfFixedPeriod 
                            |> fst

            schedule1 @ schedule2 |> List.filter (fun (_, _, pmt, _, _, _) -> pmt <= 0.00) |> Array.ofList
