
//$(document).ready(function () {
//    $('#startDate').datepicker({
//        format: 'dd-M-yyyy',
//    })
//    .change(dateChanged)
//    .on('changeDate', dateChanged);

//    $('#RemindDate').datepicker({
//        format: 'dd-M-yyyy',
//    })
//    .change(dateChanged)
//    .on('changeDate', dateChanged);


//});

//function dateChanged(ev) { $(this).datepicker('hide'); }


////FILE UPLOAD////
var videoManagement = {
    initFileUpload : function(id, fu) {
    var _url = '/Utilities/GetFile';
    var _elm = $(id);
    var _path = $(_elm).data("path");
    var _uniqueid = $(_elm).data("uniqueid");
    var _allowList = $(_elm).data("allowlist");
    var _allowMultiple = $(_elm).data("allowmultiple");
    var _videoPlayer = $(_elm).data("videoplayer");
    var _audioPlayer = $(_elm).data("audioplayer");
    var _filename = $(_elm).data("filename");
    var _filetypes = $(_elm).data("filetypes");
    var _fileLength = $(_elm).data("filelength");
    var _callback = $(_elm).data("callback");
    var _type = $(_elm).data("type");
    var _progressid = $(_elm).data("progressid");



     $(fu).fileupload({
		maxChunkSize: 999048576,
        url: _url,
        dataType: 'json',
        async: true,
        cache: false,
        add: function (e, data) {
            data.submit();
        },
        done: function (e, data) {
            //alert(data.result.Status);
            //$('#dvBackFuUpload').modal('hide');

            var callbackObj = {
                path: _path,
                uniqueid: _uniqueid,
                allowList: _allowList,
                allowMultiple: _allowMultiple,
                videoPlayer: _videoPlayer,
                audioPlayer: _audioPlayer,
                filename: data.files[0].name,
                filetypes: _filetypes,
                fileLength: _fileLength,
                callback: _callback,
            };

            if (_callback) {
                var callfun = _callback + "(" + JSON.stringify(data.result) +"," + JSON.stringify(callbackObj) + ")";
                eval(callfun);
            }

            var cancelId = "#" + _uniqueid + "_cancel";
            $(cancelId).addClass('hide');

            if (data.result.Status == '200') {
                $(_elm).find('input:text').val('d');
                $(_elm).find('input:hidden').val(data.result.Ext);
                //alert(data.result.Status);

                if (_allowList) {
                    eval('videoManagement.initFileList($(\"#dvFileList_' + _uniqueid + '\"))');
                  //  eval('initFileList($(\"#dvFileList_' + _uniqueid + '\"))');
                }

                //if (_videoPlayer.length > 0) {
                //    videoHTML(_videoPlayer, _path.substring(_path.indexOf('/') + 1) + _filename, 1);
                //}

                //if (_audioPlayer.length > 0) {
                //    audioHTML(_audioPlayer, '/' + _path + data.result.Name);
                //}

                //   $('#dvBackFuUpload').css('display', 'none'); 
                    //toastr.success('File uploaded successfully.');
               // alert('File uploaded successfully.');
            }
            else if (data.result.Status == '-200') {
             //   toastr.error('Invalid file type. Please select valid file(' + _filetypes + ')');
                //  alert('Invalid file type. Please select valid file(' + _filetypes + ')');
     //           alert('Invalid file type. Please select valid file(' + _filetypes + ')');
            }
            else if (data.result.Status == '-300') {
              //  toastr.error('Maximum upload size exceeded. Maximum allowed size for uploaded files is ' + _fileLength + ' bytes.');
                // alert('Maximum upload size exceeded. Maximum allowed size for uploaded files is ' + _fileLength + ' bytes.');
           //     alert('Maximum upload size exceeded. Maximum allowed size for uploaded files is ' + _fileLength + ' bytes.');
            }
        },
        progressall: function (e, data) {
            var progress = parseInt(data.loaded / data.total * 100, 10);
            var progressid = "#"+_progressid;
            $(progressid).css('display', 'block');
            $(progressid).css(
                'width',
                progress + '%'
            );
            progressid += "_per";
            $(progressid).html(progress + " % Complete");
          //  console.log(progress);
        },
        formData: {
            "path": _path,
            "filename": _filename,
            "filetypes": _filetypes,
            "fileLength": _fileLength,
            "allowMultiple": _allowMultiple,
            "type": _type
        }
    }).bind('fileuploadstart', function (e, data) {/* ... */
        if (_videoPlayer.length > 0) {
            videoHTML(_videoPlayer, '', 1);
        }
        //  $('#dvBackFuUpload').modal('show');
    }).bind('fileuploadadd', function (e, data) {
        var jqXHR = data.submit(); // Catching the upload process of every file
        var cancelId = "#" + _uniqueid + "_cancel";
        if ($(cancelId))
        {
            $(cancelId).removeClass('hide');
            $(cancelId).eq(-1).on("click", function () {
                if (jqXHR) {
                    jqXHR.abort();
                    jqXHR = null;
                }

                $(cancelId).addClass('hide');
            })
        }
       
    }).prop('disabled', !$.support.fileInput)
         .parent().addClass($.support.fileInput ? undefined : 'disabled');
},

    initImageUpload : function(id, fu) {
        var _url ='/Utilities/GetFile';
    var _elm = $(id);
    var _path = $(_elm).data("path");
    var _uniqueid = $(_elm).data("uniqueid");
    var _filename = $(_elm).data("filename");
    var _filetypes = $(_elm).data("filetypes");
    var _fileLength = $(_elm).data("filelength");
    
    var _filewidth = $(_elm).data("width");
    var _fileheight = $(_elm).data("height");

    if ($(_elm).find('input:hidden').val().length > 0) {
        $(_elm).find('#uploadedImage').attr('src', "/" + _path + _filename + $(_elm).find('input:hidden').val());
    }

 
    $(fu).fileupload({
        url: _url,
        dataType: 'json',
        async: true,
        cache: false,
        done: function (e, data) {

            $('#dvBackFuUpload').modal('hide');

            if (data.result.Status == '200') {
                $(_elm).find('input:hidden').val(data.result.Ext);
                $(_elm).find('#uploadedImage').attr('src', "/" + _path + data.result.Name);
                $(_elm).find('button').removeClass("dn");
            }
            else if (data.result.Status == '-200') {
                alert('Invalid file type. Please select valid file(' + _filetypes + ')');
            }
            else if (data.result.Status == '-300') {
                alert('Maximum upload size exceeded. Maximum allowed size for uploaded files is ' + _fileLength + ' bytes.');
            }
            else if (data.result.Status == '-400') {
                alert('Maximum image resolution exceeded. Maximum allowed resolution for uploaded files is ' + _filewidth + ' X ' + _fileheight + ' pixels.');
            }
        },
        progressall: function (e, data) {
            var progress = parseInt(data.loaded / data.total * 100, 10);
            $('#dvBackFuUpload .countdown-progress-bar div').css(
                'width',
                progress + '%'
            );
        },
        formData: {
            "path": _path,
            "filetypes": _filetypes,
            "fileLength": _fileLength,
            "filename": _filename,
            "filewidth": _filewidth,
            "fileheight": _fileheight
        }
    }).bind('fileuploadstart', function (e, data) {/* ... */
        $('#dvBackFuUpload').modal('show');
    }).prop('disabled', !$.support.fileInput)
         .parent().addClass($.support.fileInput ? undefined : 'disabled');
},

    initFileList:function (id) {
   
        var _url = '/Utilities/GetFileList';
    var _elm = $(id);
    var _path = $(_elm).data("path");
    // var _label = $(_elm).data("label");
    var _uniqueid = $(_elm).data("uniqueid");
    var _allowDownload = $(_elm).data("allowdownload");
    var _allowMultiple = $(_elm).data("allowmultiple");
    var _allowDelete = $(_elm).data("allowdelete");
    var _filetypes = $(_elm).data("filetypes");   
    
    $.ajax({
        type: "GET",
        url: _url,
        data: { path: _path, allowMultiple: _allowMultiple, filetypes: _filetypes },
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        cache: false,
        //error:function(a,b,c)
        //{alert(b);},
        success: function (r) {
          

            $('#' + $(_elm).attr('id') + ' > #ulFileList').empty();

            if (r.length == 0) {
                //alert('dd');
                //" + (_label != null && _label.length > 0 ? _label : 'file') + "
                //var elm2 = document.createElement("span"); //"<span>No d attached</span>";
                //var t = document.createTextNode("No d attached");
                //alert($(_elm).attr('id'));
                $('#' + $(_elm).attr('id') + ' > #ulFileList').append("<li><span></span></li>")//No file attached
            }

            $.each(r, function (index, q) {

                var elm1 = "<li >";
                //  elm1 = elm1 + "<span>" + r[index].Name + "</span>";
                elm1 = elm1 + "<span>" + _path + r[index].Name + "</span>";
                //elm1 = elm1 + "<video src='" + _path + r[index].Name + "' style='max-height:250px;max-width:250px;'></video>";
                // if (_allowMultiple) {
                if (_allowDelete) {
                    //elm1 = elm1 + "<a class='remove-file' onclick='DeleteFile(\"" + _path + "\",\"" + r[index].Name + "\",\"" + _uniqueid + "\")'>X</a>";
                    elm1 = elm1 + "<a class='remove-file' onclick=\"DeleteFile('" + _path + "\',\'" + r[index].Name + '\',\'' + _uniqueid + "')\">X</a>";
                }
                if (_allowDownload) {
   
                    elm1 = elm1 + "<a class='download-file'  onclick='SaveToDisk(\"" + _path + r[index].Name + "\", \"" + r[index].Name + "\")' ><i class='fa fa-download'></i></a>";
                }
                //} else {
                //    if (_allowDelete) {
                //        elm1 = elm1 + "<a class='remove-file' onclick='DeleteFile(\"" + _path + "\",\"\",\"" + _uniqueid + "\")'>X</a>";
                //    }
                //    if (_allowDownload) {
                //        elm1 = elm1 + "<a class='download-file'  onclick='SaveToDisk(\"" + _path + "\", \"" + r[index].Name + "\")' ><i class='fa fa-download'></i></a>";
                //    }
                //}
                elm1 = elm1 + "</li>";
                $(elm1).appendTo($('#' + $(_elm).attr('id') + ' > #ulFileList'));
                
               // console.log(JSON.stringify($('#' + $(_elm).attr('id') + ' > #ulFileList')));
               
              //  $(elm1).appendTo($('#ulFileList'));

            });

            //alert($('#' + $(_elm).attr('id') + ' > #ulFileList').find('li').length);
            //if ($('#' + $(_elm).attr('id') + ' > #ulFileList').find('li').length == 0) {
            //    alert('dd : ' + $(_elm).attr('id'));
            //   // var elm2 = "<li><span>No d attached</span></li>";
            // //  $(elm2).appendTo($('#' + $(_elm).attr('id') + ' > #ulFileList'));
            //}
        },
        error: function (jqXHR, textStatus, errorThrown) {
             
            //alert(textStatus + ' / ' + errorThrown);
        }
    });
},

    ConfirmDelete:function() {
    return confirm('Are you sure? \nYou want to delete this file');
},

    DeleteImageFile:function(path1, uploaderID) {

    if (ConfirmDelete()) {
        var url = '/Utilities/GetFileList';
        var _filename = $("#dvImageUpload_" + uploaderID).data("filename") + $("#dvImageUpload_" + uploaderID).find('input:hidden');

        $.ajax({
            type: "GET",
            url: url,
            data: { path: path1, action: 'delete', file: _filename },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (r) {
                //alert(r);
                if (r == '100') {
                    $("#dvImageUpload_" + uploaderID).find('input:hidden').val("");
                    $("#dvImageUpload_" + uploaderID).find('#uploadedImage').attr('src', '');
                    $("#dvImageUpload_" + uploaderID).find('button').addClass("dn");
                    //eval('initFileList($(\"#dvFileList_' + uploaderID + '\"))');
                    // eval('listUploadedFiles' + uploaderID + '()');
                }
                else
                    alert('Could not remove file');
            }
        });
    }
},

    DeleteFile:function(path1, fileName, uploaderID) {
    if (ConfirmDelete()) {
        var url ='/Utilities/GetFileList';
        $.ajax({
            type: "GET",
            url: url,
            data: { path: path1, action: 'delete', file: fileName },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (r) {
                //alert(r);
                if (r == '100') {
                    eval('videoManagement.initFileList($(\"#dvFileList_' + uploaderID + '\"))');
                   // eval('initFileList($(\"#dvFileList2_' + uploaderID + '\"))');
                    // eval('listUploadedFiles' + uploaderID + '()');
                }
                else
                    alert('Could not remove file');
            }
        });
    }
},

    SaveToDisk:function(fileURL, fileName) {
   
    fileURL = window.location.protocol + "//" + window.location.host + "/" + fileURL;
    // for non-IE    
    if (!window.ActiveXObject) {
        var save = document.createElement('a');
        save.href = fileURL;
        save.target = '_blank';
        save.download = fileName || 'unknown';

        var event = document.createEvent('Event');
        event.initEvent('click', true, true);
        save.dispatchEvent(event);
        (window.URL || window.webkitURL).revokeObjectURL(save.href);

        //alert(window.webkitURL.revokeObjectURL(save));
    }
        // for IE
    else if (!!window.ActiveXObject && document.execCommand) {
        var _window = window.open(fileURL, '_blank');
        _window.document.close();
        _window.document.execCommand('SaveAs', true, fileName || fileURL)
        _window.close();
    }
},

    SetFileNameLocally:function(id) {
    $('#' + id).val($('#ulFileList li:first-child span').html());
},

    initUploadedImage:function(modal) {
    //alert(modal);
    var _elm = $('#' + modal);

    var _path = $(_elm).data("path");
    var _parent = $(_elm).data("parent");
    var _filename = $(_elm).data("filename");
    var _ext = $(_elm).find('input:hidden').val();
    //alert("/content/" + _path + _filename);
    $(_elm).find('#uploadedImage').attr('src', "/content/" + _path + _filename + _ext);
},

    initUploadedAudio:function(modal) {
    //  alert(modal);
    var _elm = $('#' + modal);

    var _playbackpath = $(_elm).data("playbackpath");
    var _parent = $(_elm).data("parent");
    var _filename = $(_elm).data("filename");
    var _ext = $(_elm).find('input:hidden').val();
    //alert(_parent+' / '+_playbackpath + ' / ' + _filename);
    //videoHTML('player_upload_video_' + _parent, _playbackpath + _filename, 1);
    if (_ext != null && _ext.length > 0) {
        audioHTML('player_upload_audio_' + _parent, '/content/' + _playbackpath + _filename + _ext);
    }
},

    initUploadedVideo: function (modal) {
    //alert(parent);
    var _elm = $('#' + modal);

    var _playbackpath = $(_elm).data("playbackpath");
    var _parent = $(_elm).data("parent");
    var _filename = $(_elm).data("filename");
    var _ext = $(_elm).find('input:hidden').val();
    //alert(_parent+' / '+_playbackpath + ' / ' + _filename);
    //if (_ext != null && _ext.length > 0) {
    videoHTML('player_upload_video_' + _parent, _playbackpath + _filename + _ext, 1);
    //}
},
////FILE UPLOAD////

};

function initFileUpload(id, fu) {
    var _url = '/Utilities/GetFile';
    var _elm = $(id);
    var _path = $(_elm).data("path");
    var _uniqueid = $(_elm).data("uniqueid");
    var _allowList = $(_elm).data("allowlist");
    var _allowMultiple = $(_elm).data("allowmultiple");
    var _videoPlayer = $(_elm).data("videoplayer");
    var _audioPlayer = $(_elm).data("audioplayer");
    var _filename = $(_elm).data("filename");
    var _filetypes = $(_elm).data("filetypes");
    var _fileLength = $(_elm).data("filelength");
    var _minImageSize = $(_elm).data("minimagesize");
    var progressbarid = "#"+_uniqueid + "_dvBackFuUpload";

    $(fu).fileupload({
        maxChunkSize: 148576,
        url: _url,
        dataType: 'json',
        async: true,
        cache: false,
        add: function (e, data) {

            var maxsize = $(_elm).attr('data-filelength');
            var acceptedtype = $(_elm).attr('data-filetypes');
            maxsize = parseInt(maxsize);
            var uploadErrors = [];

            var regex = new RegExp(acceptedtype, 'gi');
            var fileExtension = "." + (data.originalFiles[0]['name']).split('.').pop();
            if (acceptedtype)
            {
                var acceptFileTypes = acceptedtype;
                if (acceptedtype.indexOf(fileExtension.toLowerCase()) == -1) {
                    uploadErrors.push('Not an accepted file type');
                }
            }
            
            if (data.originalFiles[0]['size'].length && data.originalFiles[0]['size'] > maxsize) {
                uploadErrors.push('Filesize is too big');
            }

            // console.log(data.originalFiles[0]);

            // var image = new Image();
            // //Validate the File Height and Width.
            // image.onload = function () {
                // var height = this.height;
                // var width = this.width;
                // alert("Height " + height +"======"+ width);

                // if (height > 100 || width > 100) {
                    // alert("Height and Width must not exceed 100px.");
                    // return false;
                // }
                // alert("Uploaded image has valid Height and Width.");
                // return true;
            // };
            // //Set the Base64 string return from FileReader as source.
            // image.src = data.originalFiles[0]['name'];
            // alert(e.target.result)

            // if (_minImageSize) {

            // } else {

            // }
            // alert(_minImageSize);

            if (uploadErrors.length > 0) {
                alert(uploadErrors.join("\n"));
            } else {
                data.submit();
            }
            $(progressbarid).css('display', 'none');
            // var jqXHR = data.submit();
            // .success(function (result, textStatus, jqXHR) {/* ... */})
            // .error(function (jqXHR, textStatus, errorThrown) {/* ... */})
            // .complete(function (result, textStatus, jqXHR) {/* ... */});
        },

        done: function (e, data) {
            //alert(data.result.Status);
            //$('#dvBackFuUpload').modal('hide');
            if (data.result.Status == '200') {
                $(_elm).find('input:text').val('d');
                $(_elm).find('input:hidden').val(data.result.Ext);
                //alert(data.result.Status);

                if (_allowList) {
                    eval('initFileList($(\"#dvFileList_' + _uniqueid + '\"))');
                    //  eval('initFileList($(\"#dvFileList_' + _uniqueid + '\"))');
                }

                if (_videoPlayer.length > 0) {
                    videoHTML(_videoPlayer, _path.substring(_path.indexOf('/') + 1) + _filename, 1);
                }

                if (_audioPlayer.length > 0) {
                    audioHTML(_audioPlayer, '/' + _path + data.result.Name);
                }

               // $(progressbarid).css('display', 'none');

                toastr.success('File uploaded successfully.');

            }
            else if (data.result.Status == '-200') {
                toastr.success('Invalid file type. Please select valid file(' + _filetypes + ')');
                //  alert('Invalid file type. Please select valid file(' + _filetypes + ')');
            }
            else if (data.result.Status == '-300') {
                toastr.success('Maximum upload size exceeded. Maximum allowed size for uploaded files is ' + _fileLength + ' bytes.');
                // alert('Maximum upload size exceeded. Maximum allowed size for uploaded files is ' + _fileLength + ' bytes.');
            }
        },
        progressall: function (e, data) {
            var progress = parseInt(data.loaded / data.total * 100, 10);
            $(progressbarid).css('display', 'block');
            $(progressbarid).css(
                'width',
                progress + '%'
            );

            var percentage = progressbarid + "_per";
            $(percentage).html(progress+"% Completed");
        },
        formData: {
            "path": _path,
            "filename": _filename,
            "filetypes": _filetypes,
            "fileLength": _fileLength,
            "allowMultiple": _allowMultiple
        }
    }).bind('fileuploadstart', function (e, data) {/* ... */
        if (_videoPlayer.length > 0) {
            videoHTML(_videoPlayer, '', 1);
        }
        //  $('#dvBackFuUpload').modal('show');
    }).prop('disabled', !$.support.fileInput)
        .parent().addClass($.support.fileInput ? undefined : 'disabled');
}


function initImageUpload(id, fu) {
    var _url = '/Utilities/GetFile';
    var _elm = $(id);
    var _path = $(_elm).data("path");
    var _uniqueid = $(_elm).data("uniqueid");
    var _filename = $(_elm).data("filename");
    var _filetypes = $(_elm).data("filetypes");
    var _fileLength = $(_elm).data("filelength");

    var _filewidth = $(_elm).data("width");
    var _fileheight = $(_elm).data("height");

    if ($(_elm).find('input:hidden').val().length > 0) {
        $(_elm).find('#uploadedImage').attr('src', "/" + _path + _filename + $(_elm).find('input:hidden').val());
    }


    $(fu).fileupload({
        url: _url,
        dataType: 'json',
        async: true,
        cache: false,
        done: function (e, data) {

            $('#dvBackFuUpload').modal('hide');

            if (data.result.Status == '200') {
                $(_elm).find('input:hidden').val(data.result.Ext);
                $(_elm).find('#uploadedImage').attr('src', "/" + _path + data.result.Name);
                $(_elm).find('button').removeClass("dn");
            }
            else if (data.result.Status == '-200') {
                alert('Invalid file type. Please select valid file(' + _filetypes + ')');
            }
            else if (data.result.Status == '-300') {
                alert('Maximum upload size exceeded. Maximum allowed size for uploaded files is ' + _fileLength + ' bytes.');
            }
            else if (data.result.Status == '-400') {
                alert('Maximum image resolution exceeded. Maximum allowed resolution for uploaded files is ' + _filewidth + ' X ' + _fileheight + ' pixels.');
            }
        },
        progressall: function (e, data) {
            var progress = parseInt(data.loaded / data.total * 100, 10);
            $('#dvBackFuUpload .countdown-progress-bar div').css(
                'width',
                progress + '%'
            );
        },
        formData: {
            "path": _path,
            "filetypes": _filetypes,
            "fileLength": _fileLength,
            "filename": _filename,
            "filewidth": _filewidth,
            "fileheight": _fileheight
        }
    }).bind('fileuploadstart', function (e, data) {/* ... */
        $('#dvBackFuUpload').modal('show');
    }).prop('disabled', !$.support.fileInput)
         .parent().addClass($.support.fileInput ? undefined : 'disabled');
}

function initFileList(id) {

    var _url = '/Utilities/GetFileList';
    var _elm = $(id);
    var _path = $(_elm).data("path");
    // var _label = $(_elm).data("label");
    var _uniqueid = $(_elm).data("uniqueid");
    var _allowDownload = $(_elm).data("allowdownload");
    var _allowMultiple = $(_elm).data("allowmultiple");
    var _allowDelete = $(_elm).data("allowdelete");
    var _filetypes = $(_elm).data("filetypes");
     
    $.ajax({
        type: "GET",
        url: _url,
        data: { path: _path, allowMultiple: _allowMultiple, filetypes: _filetypes },
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        cache: false,
        //error:function(a,b,c)
        //{alert(b);},
        success: function (r) {


            $('#' + $(_elm).attr('id') + ' > #ulFileList').empty();

            if (r.length == 0) {
                //alert('dd');
                //" + (_label != null && _label.length > 0 ? _label : 'file') + "
                //var elm2 = document.createElement("span"); //"<span>No d attached</span>";
                //var t = document.createTextNode("No d attached");
                //alert($(_elm).attr('id'));
                $('#' + $(_elm).attr('id') + ' > #ulFileList').append("<li><span></span></li>")//No file attached
            }

            $.each(r, function (index, q) {

                var elm1 = "<li >";
                elm1 = elm1 + "<img src='" + _path + r[index].Name + "' style='height:100px;width:100px;'/>";
               // elm1 = elm1 + "<span>" ++ r[index].Name + "</span>";

               // elm1 = elm1 + "<img src='" + _path + r[index].Name + "' style='max-height:250px;max-width:250px;'>";
                // if (_allowMultiple) {
                if (_allowDelete) {
                    //elm1 = elm1 + "<a class='remove-file' onclick='DeleteFile(\"" + _path + "\",\"" + r[index].Name + "\",\"" + _uniqueid + "\")'>X</a>";
                    elm1 = elm1 + "<a class='remove-file' onclick=\"DeleteFile('" + _path + "\',\'" + r[index].Name + '\',\'' + _uniqueid + "')\">X</a>";
                }
                if (_allowDownload) {

                    elm1 = elm1 + "<a class='download-file'  onclick='SaveToDisk(\"" + _path + r[index].Name + "\", \"" + r[index].Name + "\")' ><i class='fa fa-download'></i></a>";
                }
                //} else {
                //    if (_allowDelete) {
                //        elm1 = elm1 + "<a class='remove-file' onclick='DeleteFile(\"" + _path + "\",\"\",\"" + _uniqueid + "\")'>X</a>";
                //    }
                //    if (_allowDownload) {
                //        elm1 = elm1 + "<a class='download-file'  onclick='SaveToDisk(\"" + _path + "\", \"" + r[index].Name + "\")' ><i class='fa fa-download'></i></a>";
                //    }
                //}
                elm1 = elm1 + "</li>";
                $(elm1).appendTo($('#' + $(_elm).attr('id') + ' > #ulFileList'));

                // console.log(JSON.stringify($('#' + $(_elm).attr('id') + ' > #ulFileList')));

                //  $(elm1).appendTo($('#ulFileList'));

            });

            //alert($('#' + $(_elm).attr('id') + ' > #ulFileList').find('li').length);
            //if ($('#' + $(_elm).attr('id') + ' > #ulFileList').find('li').length == 0) {
            //    alert('dd : ' + $(_elm).attr('id'));
            //   // var elm2 = "<li><span>No d attached</span></li>";
            // //  $(elm2).appendTo($('#' + $(_elm).attr('id') + ' > #ulFileList'));
            //}
        },
        error: function (jqXHR, textStatus, errorThrown) {

            //alert(textStatus + ' / ' + errorThrown);
        }
    });
}

function ConfirmDelete() {
    return confirm('Are you sure? \nYou want to delete this file');
}


function DeleteImageFile(path1, uploaderID) {

    if (ConfirmDelete()) {
        var url = '/Utilities/GetFileList';
        var _filename = $("#dvImageUpload_" + uploaderID).data("filename") + $("#dvImageUpload_" + uploaderID).find('input:hidden');

        $.ajax({
            type: "GET",
            url: url,
            data: { path: path1, action: 'delete', file: _filename },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (r) {
                //alert(r);
                if (r == '100') {
                    $("#dvImageUpload_" + uploaderID).find('input:hidden').val("");
                    $("#dvImageUpload_" + uploaderID).find('#uploadedImage').attr('src', '');
                    $("#dvImageUpload_" + uploaderID).find('button').addClass("dn");
                    //eval('initFileList($(\"#dvFileList_' + uploaderID + '\"))');
                    // eval('listUploadedFiles' + uploaderID + '()');
                }
                else
                    alert('Could not remove file');
            }
        });
    }
}

function DeleteFile(path1, fileName, uploaderID) {
    if (ConfirmDelete()) {
        var url = '/Utilities/GetFileList';
        $.ajax({
            type: "GET",
            url: url,
            data: { path: path1, action: 'delete', file: fileName },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (r) {
                //alert(r);
                if (r == '100') {
                    eval('initFileList($(\"#dvFileList_' + uploaderID + '\"))');
                    // eval('initFileList($(\"#dvFileList2_' + uploaderID + '\"))');
                    // eval('listUploadedFiles' + uploaderID + '()');
                }
                else
                    alert('Could not remove file');
            }
        });
    }
}


function SaveToDisk(fileURL, fileName) {

    fileURL = window.location.protocol + "//" + window.location.host + "/" + fileURL;
    // for non-IE    
    if (!window.ActiveXObject) {
        var save = document.createElement('a');
        save.href = fileURL;
        save.target = '_blank';
        save.download = fileName || 'unknown';

        var event = document.createEvent('Event');
        event.initEvent('click', true, true);
        save.dispatchEvent(event);
        (window.URL || window.webkitURL).revokeObjectURL(save.href);

        //alert(window.webkitURL.revokeObjectURL(save));
    }
        // for IE
    else if (!!window.ActiveXObject && document.execCommand) {
        var _window = window.open(fileURL, '_blank');
        _window.document.close();
        _window.document.execCommand('SaveAs', true, fileName || fileURL)
        _window.close();
    }
}

function SetFileNameLocally(id) {
    $('#' + id).val($('#ulFileList li:first-child span').html());
}


function initUploadedImage(modal) {
    //alert(modal);
    var _elm = $('#' + modal);

    var _path = $(_elm).data("path");
    var _parent = $(_elm).data("parent");
    var _filename = $(_elm).data("filename");
    var _ext = $(_elm).find('input:hidden').val();
    //alert("/content/" + _path + _filename);
    $(_elm).find('#uploadedImage').attr('src', "/content/" + _path + _filename + _ext);
}


function initUploadedAudio(modal) {
    //  alert(modal);
    var _elm = $('#' + modal);

    var _playbackpath = $(_elm).data("playbackpath");
    var _parent = $(_elm).data("parent");
    var _filename = $(_elm).data("filename");
    var _ext = $(_elm).find('input:hidden').val();
    //alert(_parent+' / '+_playbackpath + ' / ' + _filename);
    //videoHTML('player_upload_video_' + _parent, _playbackpath + _filename, 1);
    if (_ext != null && _ext.length > 0) {
        audioHTML('player_upload_audio_' + _parent, '/content/' + _playbackpath + _filename + _ext);
    }
}


function initUploadedVideo(modal) {
    //alert(parent);
    var _elm = $('#' + modal);

    var _playbackpath = $(_elm).data("playbackpath");
    var _parent = $(_elm).data("parent");
    var _filename = $(_elm).data("filename");
    var _ext = $(_elm).find('input:hidden').val();
    //alert(_parent+' / '+_playbackpath + ' / ' + _filename);
    //if (_ext != null && _ext.length > 0) {
    videoHTML('player_upload_video_' + _parent, _playbackpath + _filename + _ext, 1);
    //}
}


