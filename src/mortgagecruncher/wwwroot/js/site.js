$(function() {
    $('.datepicker').datepicker({
        format: 'dd/mm/yyyy',
        autoclose: true,
        weekStart: 1
    }).on('changeDate', function(e) {
        // console.log(e);
    });
});