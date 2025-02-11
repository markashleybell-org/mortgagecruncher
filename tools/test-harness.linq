<Query Kind="Program">
  <Reference Relative="..\core\bin\Debug\netstandard2.0\core.dll">D:\Src\Personal\mortgagecruncher\core\bin\Debug\netstandard2.0\core.dll</Reference>
  <Namespace>core</Namespace>
  <Namespace>Microsoft.FSharp.Collections</Namespace>
  <RuntimeVersion>8.0</RuntimeVersion>
</Query>

void Main()
{
    var principal = 50445.00;
    var fixedRate = 4.29;
    var variableRate = 6.99;
    var term = 15.00;
    var fixedTerm = 3.00;
    
//    var overPayments = new Dictionary<double, double> {
//        // { 1, -750.00 }
//    };

	var overPayments = Enumerable.Range(1, 36).Select(i => new { p = (double)i, a = -750.00 }).ToDictionary(x => x.p, x => x.a);
	
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
