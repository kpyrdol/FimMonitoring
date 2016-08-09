var Files = {
    Init: function() {
        
    },
    Index: function () {
        //$('.import-source').bind('click', Files.SelectSource);
        $('#MainForm').bind('submit', Files.ClearSubmitForm);
        $('.pagination a').bind('click', Files.Pagination);
        $('.btn-set-status').bind('click', Files.SetStatus);
        $('.import-sources input[type="checkbox"]').bootstrapSwitch({ size: "mini", offColor: "danger", onColor : 'success'});
        $('.import-sources input[type="checkbox"]').on('switchChange.bootstrapSwitch', Files.SelectSource);
        $('.import-sources .error-level').bind('click', Files.SelectSource);
        $('input[name="importSource.Enabled"]').on('switchChange.bootstrapSwitch', Files.SetSourceStatus);
    },
    ClearSubmitForm: function(e) {
        var action = $(this).data('defaulturl');
        var form = $('#MainForm');
        form.attr('action', action);
        Files.SubmitForm(e);
        return false;
    },
    Pagination: function(e) {
        var action = $(this).attr('href');
        var form = $('#MainForm');
        form.attr('action', action);
        Files.SubmitForm(e);
        return false;
    },
    SetStatus: function (e) {
        var id = $(this).data('id');
        var status = $(this).data('status');
        bootbox.confirm("Change status?", function (result) {
            if (result) {
                var url = baseUrl + "Import/SetStatus";
                $.ajax({
                    url: url,
                    dataType: "json",
                    data: {
                        id: id,
                        status: status
                    },
                    type: 'post',
                    success: function (data) {
                        if (data.status == 'OK') {
                            Files.SubmitForm(e);
                        } else {
                            Fim.ShowError(data.error);
                        }
                    },
                    error: function () {
                        Fim.ShowError("Error while processing request");
                    }
                });
            }
        });
        return false;

    },
    SelectSource: function() {
        var state = $(this).attr('data-state');
        if (state === 'off') {
            $(this).attr('data-state', 'on');
        } else {
            $(this).attr('data-state', 'off');
        }
    },
    SubmitForm: function (e) {
        e.preventDefault();
        $('.selected-source').remove();
        $('.import-source[data-state="on"]').each(function (k, v) {
            var id = $(v).data('id');

            $('<input>').attr({
                type: 'hidden',
                name: 'SelectedSources['+k+']',
                'class': 'selected-source',
                value: id
            }).appendTo('#MainForm');

            $('<input>').attr({
                type: 'hidden',
                name: 'SourceErrors[' + k + '].SourceId',
                'class': 'selected-source',
                value : id
            }).appendTo('#MainForm');

            $('.error-level-' + id + '[data-state="on"]').each(function (kk, vv) {

                var errorLevel = $(this).data('level');
                $('<input>').attr({
                    type: 'hidden',
                    name: 'SourceErrors[' + k + '].ErrorLevels[' + kk + ']',
                    'class': 'selected-source',
                    value: errorLevel
                }).appendTo('#MainForm');
            });


        });
        Files.RefreshForm();
        return false;
    },
    RefreshForm: function () {
        var url = serverUrl + $('#MainForm').attr('action');
        $.ajax({
            url: url,
            dataType: "json",
            data: $('#MainForm').serialize(),
            type: 'post',
            success: function(data) {
                if (data.status == 'OK') {
                    $('#files').html(data.html);
                    Files.Index();
                } else {
                    Fim.ShowError(data.error);
                }
            },
            error: function() {
                Fim.ShowError("Error while processing request");
            }
        });
    },
    SetSourceStatus: function (event, state) {
        var url = baseUrl + "/Import/SetSourceStatus";
        var id = $(this).data('id');
        $.ajax({
            url: url,
            dataType: "json",
            data: {
                id: id,
                enabled : state
            },
            type: 'post',
            success: function (data) {
                if (data.status == 'OK') {
                    Fim.ShowSuccess(data.message);
                    Files.SubmitForm(event);
                } else {
                    Fim.ShowError(data.error);
                }
            },
            error: function () {
                Fim.ShowError("Error while processing request");
            }
        });
        
    }
};


$(document).ready(function() {
    
});