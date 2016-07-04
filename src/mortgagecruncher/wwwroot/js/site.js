$(function() {
    $('.datepicker').datepicker({
        format: 'dd/mm/yyyy',
        autoclose: true,
        weekStart: 1
    }).on('changeDate', function(e) {
        // console.log(e);
    });

    $('#StartDate').popover({ trigger: 'focus', title: 'TEST', content: 'This is a test' });
});