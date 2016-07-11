$(function() {
    $('.datepicker').datepicker({
        format: 'dd/mm/yyyy',
        autoclose: true,
        weekStart: 1
    }).on('changeDate', function(e) {
        // console.log(e);
    });

    var fixedTermRate = $('#FixedTermRate');
    var fixedTermRateGroup = $('#fixed-term-rate-group');

    var fixedTermYears = $('#FixedTermYears');

    fixedTermYears.on('change keyup', function(e) {
        if($(this).val() === '') {
            fixedTermRate.val('');
            fixedTermRateGroup.hide();
        } else {
            fixedTermRateGroup.show();
            fixedTermRate.val('2.95');
        }
    });

    var extraPaymentAmount = $('#ExtraPaymentAmount');
    var extraPaymentAmountGroup = $('#extra-payment-amount-group');

    var extraPaymentInterval = $('#ExtraPaymentInterval');

    extraPaymentInterval.on('change keyup', function(e) {
        if($(this).val() === '') {
            extraPaymentAmount.val('');
            extraPaymentAmountGroup.hide();
        } else {
            extraPaymentAmountGroup.show();
            extraPaymentAmount.val('0.00');
        }
    });

    var refresh = $('#refresh');

    if(refresh.val() === 'yes') {
        fixedTermYears.trigger('change'); 
        extraPaymentInterval.trigger('change');
    } else {
        refresh.val('yes');
    }
});