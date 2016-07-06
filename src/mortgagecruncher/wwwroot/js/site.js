$(function() {
    $('.datepicker').datepicker({
        format: 'dd/mm/yyyy',
        autoclose: true,
        weekStart: 1
    }).on('changeDate', function(e) {
        // console.log(e);
    });

    function setHelpText(selector, text) {
        $(selector).popover({ 
            trigger: 'focus', 
            content: text, 
            container: 'body' 
        });
    }

    setHelpText('#StartDate', 'The date your mortgage term starts from');
    setHelpText('#LoanValue', 'The total value of your mortgage loan');
    setHelpText('#TermYears', 'The number of years your mortgage runs for');
    setHelpText('#TermRate', 'The standard variable interest rate for your mortgage loan');
    setHelpText('#FixedTermYears', 'If you have a fixed rate mortgage, the number of years your rate is fixed for');
    setHelpText('#FixedTermRate', 'The interest rate for your mortgage during the fixed rate period');
    setHelpText('#ExtraPaymentInterval', 'Add a regular overpayment every N months');
    setHelpText('#ExtraPaymentAmount', 'The overpayment amount');
});