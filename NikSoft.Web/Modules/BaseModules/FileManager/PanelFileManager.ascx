<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PanelFileManager.ascx.cs" Inherits="NikSoft.Web.Modules.BaseModules.FileManager.PanelFileManager" %>
<style type="text/css">
    #filelist {
        direction: ltr;
        text-align: left;
    }
</style>
<script type="text/javascript">
    var currentpath = '';
    var listItems;
    var actoin = '';

    $(document).ready(function () {
        if (!FormData) {
            $('#btnUploadd,#btnUploadu').remove();
        } else {
            $('#oldbrowser').remove();
        }
        $('#btnDelu,#btnDeld').click(function () {
            SelectRemoveFiles();
        });
        $('#btnSavefolder').click(function () {
            var val = $('#txtfoldername').val();
            if ($.trim(val) === '') {
                Notif('Insert folder name pleas', 'error');
                return;
            }
            CreateNewFolder($.trim(val));
        });
        $('#btnCopyu,#btnCopyd').click(function () {
            SelectCopyCutFile('copy');
        });
        $('#btnMoveu,#btnMoved').click(function () {
            SelectCopyCutFile('cut');
        });
        $('#btnPasteu,#btnPasted').click(function () {
            Paste();
        });

        $('#btnopenselectfile').click(function () {
            $('#selectfile').click();
        });

        $('#selectfile').change(function () {
            var file = $(this).get(0);
            for (var i = 0; i < file.files.length; i++) {
                $('#filelist').append('<div class="col-md-12">' + file.files[i].name + " - " + (file.files[i].size / 1024).toFixed(1) + 'kb' + '</div>');
            }
        });

        $('#btnUpload').click(function () {
            var file = $('#selectfile').get(0);
            if (file.files.length === 0) {
                Notif('No file selected', 'error');
                return false;
            }
            SendFiles();
        });
    });

    $(window).load(function () {
        LoadData();
    });

    function CreateLink(item) {
        var $a = $('<a>').text(item.Name);
        if (item.IsFolder) {
            $a.addClass('btn btn-folder btn-sm').data('path', item.Path);
            $a.on('click', function () {
                LoadData($(this).data('path'));
            });
        }
        else {
            $a.addClass('btn btn-success').attr('href', item.Url).attr('target', '_blank');
        }
        return $a;
    }

    function CreateFileLink(item) {
        if (item.IsFolder) {
            return;
        }
        var $a = $('<a>').text(item.Url);
        $a.addClass('btn btn-link').attr('href', item.Url).attr('target', '_blank');
        return $a;
    }

    function CreateCheckbox(item) {
        var $cb = $('<input />', { type: 'checkbox', 'class': 'chbselect', value: item.Path })
        return $cb;
    }

    function LoadData(path) {
        if (path === undefined || $.trim(path) === '') {
            path = 'files';
        }
        currentpath = path;
        var dataParameters = "{'path':'" + path + "'}";
        $.ajax({
            type: "POST",
            async: false,
            url: "../../../WebService/PortalWebService.asmx/LoadFileList",
            data: dataParameters,
            contentType: "application/json; charset=utf-8",
            success: function (msg) {
                $("#filetable tr:has(td):not(:has(th))").remove();
                $.each($.parseJSON(msg.d), function (index, item) {
                    $('<tr>').append(
									$('<td>').html(index + 1 + '.'),
									$('<td>').html(CreateCheckbox(item)),
									$('<td>').html(CreateLink(item)),
									$('<td>').html(CreateFileLink(item)),
									$('<td>').text(item.CreateDate),
									$('<td>').text(item.Size)
								).appendTo('#filetable');
                });
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert(xhr.responseText);
                alert(thrownError);
            }
        });
    }

    function RemoveFiles(varitems) {
        var dataParameters = { items: varitems };
        $.ajax({
            type: "POST",
            async: false,
            url: "../../../WebService/PortalWebService.asmx/RemoveFiles",
            data: JSON.stringify(dataParameters),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (msg) {
                if (msg.d === 'true' || msg.d === true) {
                    Notif('Remove is success', 'success');
                }
                LoadData(currentpath);
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert(xhr.responseText);
                alert(thrownError);
            }
        });
    }

    function SelectRemoveFiles() {
        var $chbs = $('#filetable .chbselect:checked');
        if ($chbs.length === 0) {
            Notif('No file selected', 'error');
            return;
        }
        var n = new Noty({
            text: 'Are you sure to delete?',
            type: 'error',
            layout: 'center',
            buttons: [
                Noty.button('OK', 'btn btn-default btn-save-nik', function () {
                    var items = $chbs.map(function () { return $(this).val(); }).get().join(",").split(',');
                    RemoveFiles(items);
                    n.close();
                },
	            { id: 'btnRemoveConf', 'data-status': 'ok' }),
                Noty.button('Cansel', 'btn btn-default btn-back', function () {
                    n.close();
                })
            ]
        }).show();
    }

    function CreateNewFolder(folderName) {
        var dataParameters = "{folderName:'" + folderName + "',path:'" + currentpath + "'}";
        $.ajax({
            type: "POST",
            async: false,
            url: "../../../WebService/PortalWebService.asmx/CreateNewFolder",
            data: dataParameters,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (msg) {
                if (msg.d === 0) {
                    $('#createfolder').modal('hide');
                    $('#txtfoldername').val('');
                    Notif('Folder save successfull', 'success');
                    LoadData(currentpath);
                } else if (msg.d === 1) {
                    Notif('Exist same name folder', 'error');
                } else if (msg.d === 2) {
                    Notif('The folder saving faild', 'error');
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert(xhr.responseText);
                alert(thrownError);
            }
        });
    }

    function SelectCopyCutFile(ac) {
        var $chbs = $('#filetable .chbselect:checked');
        if ($chbs.length === 0) {
            Notif('No file selected', 'error');
            return;
        }
        listItems = $chbs.map(function () { return $(this).val(); }).get().join(",").split(',');
        actoin = ac;
        Notif(listItems.length + ' Of file selected', 'information');
    }

    function Paste() {
        if (listItems === undefined || listItems.length === 0 || actoin === '') {
            Notif('No file selected', 'error');
            return;
        }
        var dataParameters = { items: listItems, path: currentpath, action: actoin };
        $.ajax({
            type: "POST",
            async: false,
            url: "../../../WebService/PortalWebService.asmx/PasteFiles",
            data: JSON.stringify(dataParameters),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (msg) {
                if (msg.d === 'true' || msg.d === true) {
                    Notif('Attachment is success', 'success');
                }
                LoadData(currentpath);
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert(xhr.responseText);
                alert(thrownError);
            }
        });
        listItems = undefined;
        actoin = '';
    }

    function SendFiles() {
        $('#uploadprogress').css({ width: 0 });
        var url = "../../../WebService/PortalWebService.asmx/UploadFile";
        if (!FormData) {
            Notif('Use update browser', 'error');
            return false;
        }

        var fd = new FormData();
        var file = document.getElementById('selectfile');
        for (var i = 0; i < file.files.length; i++) {
            fd.append('file', file.files[i]);
        }

        var xhr = new XMLHttpRequest();
        xhr.open('post', url, true);
        xhr.setRequestHeader("Path", currentpath);
        xhr.upload.addEventListener("progress", progressHandler, false);
        xhr.onreadystatechange = function () {
            if (xhr.readyState == 4 && xhr.status == 200) {
                $('#filelist').empty();
                $('#selectfile').val('');
                LoadData(currentpath);
            }
        };
        xhr.send(fd);

        return false;
    }
    function progressHandler(oEvent) {
        if (oEvent.lengthComputable) {
            var percentComplete = (oEvent.loaded / oEvent.total) * 100;
            $('#uploadprogress').css({ width: percentComplete + '%' });
        } else {
            $('#uploadprogress').hide();
        }
    }


    function Notif(Message, MTpye) {
        new Noty({
            text: Message,
            type: MTpye,
            timeout: 4000,
            layout: 'topRight',
            theme: 'mint'
        }).show();
    }

</script>
<div class="alert alert-warning" id="oldbrowser">
    Use update Browser Pleas
</div>
<div class="row">
    <div class="col-md-12">
        <button role="button" id="btnCrtu" type="button" class="btn btn-default btn-create btn-sm" data-toggle="modal" data-target="#createfolder">New Folder <span class="glyphicon glyphicon-folder-open"></span></button>
        <button role="button" id="btnCopyu" type="button" class="btn btn-default btn-edite btn-sm">Copy</button>
        <button role="button" id="btnMoveu" type="button" class="btn btn-default btn-search btn-sm">Move</button>
        <button role="button" id="btnPasteu" type="button" class="btn btn-default btn-back btn-sm">Attach</button>
        <button role="button" id="btnDelu" type="button" class="btn btn-default btn-delete btn-sm">Delete</button>
        <button role="button" id="btnUploadu" type="button" class="btn btn-default btn-create btn-sm" data-toggle="modal" data-target="#uploadmodal">Upload</button>
    </div>
</div>
<hr />
<div class="row">
    <div class="col-md-12">
        <table id="filetable" class="table table-striped table-bordered table-hover table-responsive">
            <tr>
                <th>Row</th>
                <th></th>
                <th>File name</th>
                <th>Path</th>
                <th>Create Date</th>
                <th>Volume</th>
            </tr>
            <tr></tr>
        </table>
    </div>
</div>
<hr />
<div class="row">
    <div class="col-md-12">
        <button role="button" id="btnCrtd" type="button" class="btn btn-default btn-create btn-sm" data-toggle="modal" data-target="#createfolder">New Folder <span class="glyphicon glyphicon-folder-open"></span></button>
        <button role="button" id="btnCopyd" type="button" class="btn btn-default btn-edite btn-sm">Copy</button>
        <button role="button" id="btnMoved" type="button" class="btn btn-default btn-search btn-sm">Move</button>
        <button role="button" id="btnPasted" type="button" class="btn btn-default btn-back btn-sm">Attach</button>
        <button role="button" id="btnDeld" type="button" class="btn btn-default btn-delete btn-sm">Delete</button>
        <button role="button" id="btnUploadd" type="button" class="btn btn-default btn-create btn-sm" data-toggle="modal" data-target="#uploadmodal">Upload</button>
    </div>
</div>
<div class="modal fade" id="createfolder" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-body">
                <div class="row">
                    <div class="col-md-12">
                        <div class="form-group">
                            <label class="control-label" for="">Folder Name</label>
                            <input type="text" class="form-control" id="txtfoldername" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="modal-footer">
                <button id="btnSavefolder" type="button" class="btn btn-default btn-save-nik btn-sm">Save</button>
                <button type="button" class="btn btn-default btn-back btn-sm" data-dismiss="modal">Back</button>
            </div>
        </div>
    </div>
</div>
<div class="modal fade" id="uploadmodal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-body">
                <div class="row">
                    <div class="col-md-12">
                        <div class="form-group">
                            <input type="file" id="selectfile" multiple="multiple" style="display: none;" />
                            <button role="button" type="button" id="btnopenselectfile" class="btn btn-primary">Select File</button>
                        </div>
                    </div>
                </div>
                <div class="row" id="filelist">
                </div>
                <div class="progress progress-striped active">
                    <div id="uploadprogress" class="progress-bar progress-bar-success" role="progressbar" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100" style="width: 0;">
                        <span class="sr-only"></span>
                    </div>
                </div>
            </div>
            <div class="modal-footer">
                <button id="btnUpload" type="button" class="btn btn-default btn-save-nik btn-sm">Upload</button>
                <button type="button" class="btn btn-default btn-back btn-sm" data-dismiss="modal">Back</button>
            </div>
        </div>
    </div>
</div>