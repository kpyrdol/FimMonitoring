var Fim = {
    ShowSuccess: function (msg) {
        $("#successAlert").html(msg).fadeIn(1000).delay(3000).fadeOut();
    },
    ShowError: function (msg) {
        $("#errorAlert").html(msg).fadeIn(1000).delay(3000).fadeOut();
    },
    Init: function() {
        $.ajaxSetup({
            'beforeSend': function (xhr) {
                var securityToken = $('[name=__RequestVerificationToken]').val();
                xhr.setRequestHeader('__RequestVerificationToken', securityToken);
            }
        });
        $('body').on('hidden.bs.modal', '.modal', function () {
            $(this).find("div.modal-body").html("");
            $(this).removeData('bs.modal');
        });

        $("#DetailsModal").on("shown", function () {
            console.log(Math.floor(Math.random() * 101));
        });

        $(document).ajaxStart(function () { Pace.restart(); });

        $('[data-toggle="popover"]').popover();

        //$('input').iCheck({
        //    checkboxClass: 'icheckbox_minimal-blue',
        //    radioClass: 'iradio_minimal-blue',
        //});
    }
}

$(document).ready(function () {
    Fim.Init();
});