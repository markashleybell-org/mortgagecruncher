<Query Kind="FSharpProgram">
  <Reference Relative="bin\Debug\netcoreapp1.1\MortgageCruncher.Core.dll">E:\Src\mortgagecruncher\src\MortgageCruncher.Core\bin\Debug\netcoreapp1.1\MortgageCruncher.Core.dll</Reference>
  <NuGetReference Prerelease="true">FSharp.Core</NuGetReference>
  <NuGetReference Prerelease="true">Microsoft.FSharp.Core.netcore</NuGetReference>
  <Namespace>MortgageCruncher.Core</Namespace>
  <Namespace>System</Namespace>
</Query>

let scheduleType = FixedPayments
let loanValue = 116250.00M
let mortgageTerm = 25 * 12
let mortgageStartDate = new DateTime(2015, 6, 25)
let standardInterestRate = { Type=Variable; Rate=4.49M }
let fixedRateTerm = 5 * 12
let fixedInterestRate = { Type=Fixed; Rate=2.95M }
let overPayment = 100.00M

let ft = AmortisationSchedule.create FixedTerm loanValue mortgageTerm mortgageStartDate standardInterestRate.Rate fixedRateTerm fixedInterestRate.Rate 0M
let fto = AmortisationSchedule.create FixedTerm loanValue mortgageTerm mortgageStartDate standardInterestRate.Rate fixedRateTerm fixedInterestRate.Rate overPayment
let fp = AmortisationSchedule.create FixedPayments loanValue mortgageTerm mortgageStartDate standardInterestRate.Rate fixedRateTerm fixedInterestRate.Rate 0M
let fpo = AmortisationSchedule.create FixedPayments loanValue mortgageTerm mortgageStartDate standardInterestRate.Rate fixedRateTerm fixedInterestRate.Rate overPayment

// Visualisation code

let rec last = function
    | hd :: [] -> hd
    | hd :: tl -> last tl
    | _ -> failwith "Empty list."

let headTail (list:AmortisationScheduleEntry list) = 
    [list.Head; (last list)]
    
let DumpAs (name:string) x =
    x.Dump name
    
let Peek (name:string) x =
    x.Dump name
    x

let fmtYear (d:DateTime) = d.ToString("yyyy")
let fmtDate (d:DateTime) = d.ToString("dd MMM")
let fmtDecimal (n:decimal) = n.ToString("0.00")

type DisplayAmortisationScheduleEntry = {
    Year:string
    Date:string
    No:int
    IntType:string
    IntRate:string
    Payment:string
    Interest:string
    Principal:string
    Balance:string
//    LV:string
//    TM:int
}

let visualise (amSchedule:AmortisationScheduleEntry list) = 
    amSchedule
    |> List.map (fun e -> { Year=(fmtYear e.PaymentDate) 
                            Date=(fmtDate e.PaymentDate) 
                            No=e.PaymentNumber 
                            IntType=(match e.InterestRate.Type with
                                     | Fixed -> "Fixed"
                                     | Variable -> "Variable")
                            IntRate=(fmtDecimal e.InterestRate.Rate)
                            Payment=(fmtDecimal e.Payment)
                            Interest=(fmtDecimal e.Interest)
                            Principal=(fmtDecimal e.Principal)
                            Balance=(fmtDecimal e.Balance)
//                            LV=(fmtDecimal e.LoanValue)
//                            TM=e.TermMonths 
                            })
                            
ft |> visualise |> DumpAs "FixedTerm" |> ignore
fto |> visualise |> DumpAs "FixedTerm w/Overpayment" |> ignore
fp |> visualise |> DumpAs "FixedPayments" |> ignore
fpo |> visualise |> DumpAs "FixedPayments w/Overpayment" |> ignore