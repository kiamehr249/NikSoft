$(document).ready(function () {

    //**************************************
    //select dropdown
    //*************************************

    $('.select2').select2();


    //Grid CheckBox Select All
    $("input[id*=chan]").click(function () {
        var id = $(this).attr('id').substring(4);
        $('input[name="ch1' + id + '"]').not("input[id*=chan]").prop("checked", $(this).prop('checked'));
        deletebuttonchecker("ch1", "chan");
    });

    $('input[name*="ch1"]').click(function () {
        deletebuttonchecker("ch1", "chan");
    });


    $(document).ajaxStart(function () {
        
    });
    $(document).ajaxStop(function () {
        $('.ajaxloading').remove();
    });
    $(document).ajaxError(function () {
        $('.ajaxloading').remove();
    });


});

function deletebuttonchecker(ch1Name, chName) {
    var isanychecked = false;
    var checkedcount = 0;
    $('input[name*="' + ch1Name + '"]').each(function (index) {
        if ($(this).prop('checked')) {
            isanychecked = true;
            checkedcount++;
        }
    });
    if (isanychecked) {
        $('#bd').removeAttr('disabled');
        $('#' + chName).prop("checked", true);
    } else {
        $('#bd').attr('disabled', 'disabled');
        $('#' + chName).prop("checked", false);
    }
    if (checkedcount === 1) {
        $('#be').removeAttr('disabled');
    }
    else {
        $('#be').attr('disabled', 'disabled');
    }
}

//ckeck the inputs if null
function chekEmpty() {
    var inputs = $('input.requir,select.requir,textarea.requir'), empty = false;
    inputs.each(function () {
        if ($(this).val() === '') {
            empty = true;
            $(this).css('background', '#ff517c');
            return;
        }
        else $(this).css('background', '#ddef97');
    });

    var selects = $('select.requir,select.numberrequir');
    selects.each(function () {
        if ($(this).val() === '0') {
            empty = true;
            $(this).css('background', '#ff517c');
            return;
        }
        else $(this).css('background', '#ddef97');
    });

    if (empty) {
        alert('Pleas complite them');
        return false;
    } else {
        return confirm('Are you Ready?');
    }
}

//*******************************
// Start DropDownList Funtion
//******************************



function CallWebService(serviceName, functionName, dataParameters, ddl, selectedvalue) {
    $.ajax({
        type: "POST",
        async: false,
        url: "../../WebService/" + serviceName + "/" + functionName,
        data: dataParameters,
        contentType: "application/json; charset=utf-8",
        success: function (msg) {
            SetDdlItems(ddl, msg.d);
            SetDdlValue(ddl, selectedvalue);
        },
        error: function (xhr, ajaxOptions, thrownError) {
            console.log(xhr.responseText);
            console.log(thrownError);
        }
    });
}


function SetJsonDdlItems(ddl, JsonString) {
    ddl.html('');
    $.each(JsonString, function (index, resultedObject) {
        $(ddl).append("<option value='" + resultedObject.ID + "'>" + resultedObject.Title + "</option>");
    });
    if ($(ddl).hasClass('notsearchable') === false) {
        //if ($(ddl).hasClass("select2-hidden-accessible"))
        $(ddl).select2('destroy');
        $(ddl).select2();
    }
}


function CallWebServices(serviceurl, dataParameters, ddl, selectedvalue) {
    $.ajax({
        type: "POST",
        async: false,
        url: serviceurl,
        data: dataParameters,
        contentType: "application/json; charset=utf-8",
        success: function (msg) {
            SetDdlItems(ddl, msg.d);
            SetDdlValue(ddl, selectedvalue);
        },
        error: function (xhr, ajaxOptions, thrownError) {
            console.log(xhr.responseText);
            console.log(thrownError);
        }
    });
}

function FirstOnChange(ddl1, ddl2, functionToCall, opt1, opt2, opt3) {
    $(ddl1).bind('change', function () {
        if ($(this).find("option:selected").index() > 0) {
            functionToCall($(this).val(), opt1 === undefined ? opt1 : opt1.val(), opt2 === undefined ? opt2 : opt2.val(), opt3 === undefined ? opt3 : opt3.val());
        } else {
            ClearDdl(ddl2);
        }
    });
}
function SetDdlItems(ddl, JsonString) {
    var options = $.parseJSON(JsonString);
    ddl.html('');
    $.each(options, function (index, resultedObject) {
        $(ddl).append("<option value='" + resultedObject.ID + "'>" + resultedObject.Title + "</option>");
    });
    if ($(ddl).hasClass('notsearchable') === false) {
        $(ddl).select2('destroy');
        $(ddl).select2();
    }
}

function ClearDdl(ddl) {
    $(ddl).html('');
    if ($(ddl).hasClass('notsearchable') === false) {
        $(ddl).select2('destroy');
        $(ddl).select2();
    }
}

function SetDdlValue(ddl, value) {
    if (value) {
        $(ddl).val(value);
        if ($(ddl).hasClass('notsearchable') === false) {
            $(ddl).select2('destroy');
            $(ddl).select2();
        }
    }
}
//****************************************************
//End ddl service
//****************************************************

