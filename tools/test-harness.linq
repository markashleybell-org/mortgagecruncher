<Query Kind="Program">
  <Reference Relative="..\core\bin\Debug\core.dll">C:\Src\mortgagecruncher\core\bin\Debug\core.dll</Reference>
  <Reference Relative="..\core\bin\Debug\ExcelFinancialFunctions.dll">C:\Src\mortgagecruncher\core\bin\Debug\ExcelFinancialFunctions.dll</Reference>
  <Reference Relative="..\core\bin\Debug\FSharp.Core.dll">C:\Src\mortgagecruncher\core\bin\Debug\FSharp.Core.dll</Reference>
  <Reference Relative="..\core\bin\Debug\System.ValueTuple.dll">C:\Src\mortgagecruncher\core\bin\Debug\System.ValueTuple.dll</Reference>
  <Namespace>core</Namespace>
  <Namespace>Microsoft.FSharp.Collections</Namespace>
</Query>

void Main()
{
    var principal = 116250.00;
    var fixedRate = 2.95;
    var variableRate = 4.49;
    var term = 25.00;
    var fixedTerm = 5.00;
    
    var overPayments = new Dictionary<double, double> {
        //{ 26.00, -500.00 }
    };
    
    Func<Tuple<int, InterestRate, double, double, double, double>, object> transform = row => {
        var (per, rate, pmt, ipmt, ppmt, bal) = row;
        return new { 
            period = per,
            rateType = rate.Type.ToString(),
            rate = rate.APR,
            payment = pmt.ToString("0.00"),
            interest = ipmt.ToString("0.00"),
            principal = ppmt.ToString("0.00"),
            balance = bal.ToString("0.00")
        };
    };
    
    var fixedPaymentSchedule = AmortisationSchedule.create(
        term, 
        principal, 
        ScheduleType.FixedPayments, 
        variableRate, 
        fixedTerm, 
        fixedRate, 
        overPayments
    ).Select(transform);

    var fixedTermSchedule = AmortisationSchedule.create(
        term, 
        principal, 
        ScheduleType.FixedTerm, 
        variableRate, 
        fixedTerm, 
        fixedRate, 
        overPayments
    ).Select(transform);
    
    new List<object> { new { FixedTerm = fixedTermSchedule, FixedPayment = fixedPaymentSchedule } }.Dump();
}
