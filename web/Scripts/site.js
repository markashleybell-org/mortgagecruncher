$(function () {
    $('.datepicker').datepicker({
        format: 'dd/mm/yyyy',
        autoclose: true,
        weekStart: 1
    }).on('changeDate', function (e) {
        // console.log(e);
    });

    var fixedTermRate = $('#FixedTermRate');
    var fixedTermRateGroup = $('#fixed-term-group');

    var fixedTermYears = $('#FixedTermYears');

    fixedTermYears.on('change keyup', function (e) {
        if ($(this).val() === '') {
            fixedTermRate.val('');
            fixedTermRateGroup.hide();
        } else {
            fixedTermRateGroup.show();
            fixedTermRate.val('2.95');
        }
    });

    var extraPaymentAmount = $('#OverpaymentAmount');
    var extraPaymentAmountGroup = $('#overpayment-group');

    var extraPaymentInterval = $('#OverpaymentInterval');

    extraPaymentInterval.on('change keyup', function (e) {
        if ($(this).val() === '') {
            extraPaymentAmount.val('');
            extraPaymentAmountGroup.hide();
        } else {
            extraPaymentAmountGroup.show();
        }
    });

    var refresh = $('#refresh');

    if (refresh.val() === 'yes') {
        fixedTermYears.trigger('change');
        extraPaymentInterval.trigger('change');
    } else {
        refresh.val('yes');
    }

    var form = $('#form-variables');
    var summary = $('#mortgage-summary');

    $('#show-form').on('click', function (e) {
        form.show();
        summary.hide();
    });
});