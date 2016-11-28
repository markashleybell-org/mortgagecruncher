namespace MortgageCruncher.Core

open System

type AmortisationScheduleType = FixedTerm | FixedPayments

type InterestRateType = Fixed | Variable

type InterestRate = { 
    Type:InterestRateType
    Rate:decimal }

type AmortisationScheduleEntry = { 
    PaymentDate:DateTime
    PaymentNumber:int
    InterestRate:InterestRate
    Payment:decimal
    Principal:decimal
    Interest:decimal
    Balance:decimal
    LoanValue:decimal
    TermMonths:int }

module AmortisationSchedule = 
    let private calculateMonthlyInterestRate r = 
        r.Rate / 100.00M / 12.00M

    let private calculateTermMonths mortgageTerm variableRateTerm interestRateType =
        match interestRateType with
        | Fixed -> mortgageTerm 
        | Variable -> variableRateTerm

    let private calculateMonthlyPayment interestRate termMonths amount = 
        let a = pown (1.00M + interestRate) termMonths
        amount * ((interestRate * a) / (a - 1.00M))
        
    let private calculateMonthlyInterest interestRate balance = 
        interestRate * balance
        
    let private updateEntryTotals payment interest balance loanValue termMonths entry = 
        { entry with Payment=payment; Principal=payment-interest; Interest=interest; Balance=balance; LoanValue=loanValue; TermMonths=termMonths }

    let private paymentAndInterest loanValue termMonths interestRate balance = 
        let monthlyInterestRate = calculateMonthlyInterestRate interestRate
        let payment = calculateMonthlyPayment monthlyInterestRate termMonths loanValue
        let interest = calculateMonthlyInterest monthlyInterestRate balance
        (payment, interest)

    let private updateEntry balance interest payment overPayment fallback loanValue termMonths entry =
        let p2 = if (payment + overPayment) <= balance then payment + overPayment else fallback
        let updatedBalance = ((balance - p2) + interest)
        entry |> updateEntryTotals p2 interest updatedBalance loanValue termMonths

    let private updateLoanValue loanValue balance condition = 
        if condition then balance else loanValue

    let private calculateFixedTermEntryTotals (mortgageTerm, fixedRateTerm, variableRateTerm, loanValue, termMonths, balance, overPayment, entries) entry = 
        let (payment, interest) = paymentAndInterest loanValue termMonths entry.InterestRate balance
        let updatedEntry = entry |> updateEntry balance interest payment overPayment payment loanValue termMonths
        let updatedTermMonths = if overPayment > 0M 
                                then mortgageTerm - entry.PaymentNumber
                                else 
                                    if entry.PaymentNumber = fixedRateTerm 
                                    then variableRateTerm
                                    else termMonths
        let updatedLoanValue  = updateLoanValue loanValue updatedEntry.Balance (entry.PaymentNumber = fixedRateTerm || overPayment > 0M)
        (mortgageTerm, fixedRateTerm, variableRateTerm, updatedLoanValue, updatedTermMonths, updatedEntry.Balance, overPayment, updatedEntry::entries)

    let private calculateFixedPaymentEntryTotals (mortgageTerm, fixedRateTerm, variableRateTerm, loanValue, termMonths, balance, overPayment, entries) entry = 
        let updatedTermMonths = calculateTermMonths mortgageTerm variableRateTerm entry.InterestRate.Type
        let (payment, interest) = paymentAndInterest loanValue updatedTermMonths entry.InterestRate balance
        let updatedEntry = entry |> updateEntry balance interest payment overPayment (balance + interest) loanValue updatedTermMonths
        let updatedLoanValue  = updateLoanValue loanValue updatedEntry.Balance (entry.PaymentNumber = fixedRateTerm)
        (mortgageTerm, fixedRateTerm, variableRateTerm, updatedLoanValue, updatedTermMonths, updatedEntry.Balance, overPayment, updatedEntry::entries)

    let private createSchedule mortgageTerm (mortgageStartDate:DateTime) standardInterestRate fixedRateTerm fixedInterestRate = 
        [1..mortgageTerm] 
        |> List.map (fun i -> { PaymentDate=mortgageStartDate.AddMonths(i) 
                                PaymentNumber=i
                                InterestRate=if i <= fixedRateTerm 
                                             then fixedInterestRate 
                                             else standardInterestRate
                                Payment=0M
                                Principal=0M
                                Interest=0M
                                Balance=0M
                                LoanValue=0M 
                                TermMonths=0 })

    let create scheduleType loanValue mortgageTerm mortgageStartDate standardInterestRate fixedRateTerm fixedInterestRate overPayment =
        let standardRate = { Type=Variable; Rate=standardInterestRate }
        let fixedRate = { Type=Fixed; Rate=fixedInterestRate }
        let variableRateTerm = mortgageTerm - fixedRateTerm 
        let schedule = createSchedule mortgageTerm mortgageStartDate standardRate fixedRateTerm fixedRate
        let calcFunction = match scheduleType with
                           | FixedTerm -> calculateFixedTermEntryTotals
                           | FixedPayments -> calculateFixedPaymentEntryTotals
        let initialState = (mortgageTerm, fixedRateTerm, variableRateTerm, loanValue, mortgageTerm, loanValue, overPayment, [])                           
        let (_, _, _, _, _, _, _, entries) = schedule
                                           |> List.fold calcFunction initialState
        entries
        |> List.rev
        |> List.filter (fun e -> e.Payment > 0M)
