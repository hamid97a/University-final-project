
function DetailsShowData($id, url, $PanelContentDetails, successFunction) {
    successFunction = typeof successFunction !== 'undefined' ? successFunction : function () { };
    var temp = url + `/${$id}`;
    DataAjaxAnimate($PanelContentDetails, temp, successFunction);
}

function DataAjaxAnimate($PanelContentDetails, $url, successFunction) {
    successFunction = typeof successFunction !== 'undefined' ? successFunction : function () { };
    //console.log($PanelContentDetails);
    //برای اولین باری که لود میکنیم
    if ($($PanelContentDetails).html().trim() == "") {
        var loadingText =
            '<div class="p-20 bg-orange"><i class="fa fa-refresh fa-spin"></i><span class="m-10">در حال بارگذاری ... </span></div>';
        $($PanelContentDetails).html(loadingText);
        $($PanelContentDetails).show(1000);
    }

    $.ajax({
        type: "POST",
        url: $url,
        success: function (data) {
            modalAlert('جزئیات پرونده', data);
        }
    });
}

function ConvertNumberToPersion() {
    let persian = { 0: '۰', 1: '۱', 2: '۲', 3: '۳', 4: '۴', 5: '۵', 6: '۶', 7: '۷', 8: '۸', 9: '۹' };
    function traverse(el) {
        if (el.nodeType == 3) {
            var list = el.data.match(/[0-9]/g);
            if (list != null && list.length != 0) {
                for (var i = 0; i < list.length; i++)
                    el.data = el.data.replace(list[i], persian[list[i]]);
            }
        }
        for (var i = 0; i < el.childNodes.length; i++) {
            traverse(el.childNodes[i]);
        }
    }
    traverse(document.body);
}

function SetGridCookie(GridName, CookieName, Perm) {
    $("#" + GridName).trigger("resize");
    if (Perm) {
        jQuery("#" + GridName).jqGrid("remapColumns", Perm, true);
        cols = $("#" + GridName).jqGrid('getGridParam', "colModel");
        colChosen = [];
        for (i = 0; i < cols.length; i++) {
            if (!cols[i]['hidden']) {
                colChosen.push(cols[i].name);
            }
        }
        var columns = colChosen.join(",");
        createCookie(CookieName, columns, 1000);
    }
}

function GetGridCookie(GridName, CookieName) {
    if (readCookie(CookieName) != null) {
        var EstateGridCol = readCookie(CookieName).split(',');
        var columnNames = $("#" + GridName).jqGrid('getGridParam', "colModel");
        jQuery.each(columnNames,
            function (i, item) {
                var showHide = 0;
                jQuery.each(EstateGridCol,
                    function (j, val) {
                        if (val == item.name) {
                            showHide = 1;
                        }
                    });
                if (showHide == 1) {
                    jQuery("#" + GridName).jqGrid('showCol', [item.name]);
                } else {
                    jQuery("#" + GridName).jqGrid('hideCol', [item.name]);
                }
            });
    }
}

function createCookie(name, value, days) {
    var expires = "";
    if (days) {
        var date = new Date();
        date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
        expires = "; expires=" + date.toGMTString();
    }
    var cookie = encodeURIComponent(name) + "=" + encodeURIComponent(value) + expires + "; path=/;SameSite=strict";
    document.cookie = cookie;
}

function readCookie(name) {
    var nameEQ = encodeURIComponent(name) + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) === ' ') c = c.substring(1, c.length);
        if (c.indexOf(nameEQ) === 0) return decodeURIComponent(c.substring(nameEQ.length, c.length));
    }
    return null;
}
function eraseCookie(name) {
    document.cookie = name + "=; expires=Thu, 01 Jan 1970 00:00:01 GMT;";
}

function createJsonCookie(name, json, day) {
    var defaultLength = 1800;

    var value = JSON.stringify(json);

    if (value.length < defaultLength) {
        createCookie(name, value, day);
        return;
    }
    var lastLoop = (value.length / defaultLength);
    for (var index = 0; index < lastLoop; index++) {

        var start = index * defaultLength;
        var end = (index + 1) * defaultLength;
        end = (end > value.length) ? value.length : end;

        var cookieVal = value.substring(start, end);
        createCookie(name + "_" + index, cookieVal, day);
    }
}

function readJsonCookie(name) {
    var value = readCookie(name);
    if (value != null)
        return value;

    value = "";
    var index = 0;
    while (true) {
        var partValue = readCookie(name + "_" + index);
        if (partValue == null)
            break;
        index++;
        value += partValue;

    }
    if (value === "")
        return null;
    var result = JSON.parse(value);
    return result;
}

function modalAlert($title, $message) {
    $modalHtmlEditor = '#SystemMessage';
    $modalHtmlEditorTextSpan = '#SystemMessageText';
    $modalHtmlEditorTitleSpan = '#SystemMessageTitle';
    $modalHtmlEditorTitleColor = "#SystemMessageColor";

    if ($message.search("نشد") != -1 || $message.search("خطا") != -1) {
        $($modalHtmlEditorTitleColor).attr('class', 'modal-header bg-danger');
        $title = "<i class='fa fa-exclamation-triangle m-l-5' aria-hidden='true' ></i>" + $title;
    }
    else if ($message.search("شد") != -1 || $message.search("موفق") != -1) {
        $($modalHtmlEditorTitleColor).attr('class', 'modal-header bg-success');
        $title = "<i class='fa fa-check m-l-5' aria-hidden='true' ></i>" + $title;

    } else {
        $($modalHtmlEditorTitleColor).attr('class', 'modal-header bg-info');
    }

    $($modalHtmlEditorTextSpan).html($message);
    $($modalHtmlEditorTitleSpan).html($title);

    if ($message.length > 0)
        $($modalHtmlEditor).modal('show');
    else
        console.log("modalAlert running withot any message");
}

//AJAX Setup and Related functions////////////////////////////////////////////////
function LoaderStart() {
    $("#loaderContent").fadeIn(200);
}
function LoaderExit() {
    $("#loaderContent").delay(50).fadeOut(100);
    ConvertNumberToPersion
}

function AjaxErrorHandeling(xhr, status, error) {
    $ErrorCode = xhr.status;
    //console.log('BCO Ajax Error: ');
    //console.log($(xhr.responseText));
    $message = "خطا!";
    switch ($ErrorCode) {
        case '404':
        case 404:
            $message = "آدرس ارسالی نادرست است";
            break;
        case '500':
        case 500:
            $message = "خطا در پردازش صفحه دریافتی";
            break;
        default: return 0;
            //$message = "لطفا با پشتیبانی تماس بگیرد. کدخطا : " + $ErrorCode;
            break;
    }
    modalAlert("پیام سیستم", $message + '<span style="color:#fff">' + xhr.url + "</span>");
}
var timer = null;
$.ajaxSetup({
    type: "GET",
    traditional: true,
    //async: false,
    cache: true,
    context: document.body,
    timeOut: 90000,
    error: function (xhr, status, error) {
        AjaxErrorHandeling(xhr, status, error);
        LoaderExit();
    }
});
var AjaxTimingitem = [];
$(document).ajaxSend(function (evt, request, settings) {
    request.url = settings.url;
    //console.log(request.url);
    if (timer) { clearTimeout(timer); }
    timer = setTimeout(function () { LoaderStart(); }, 1200);

    var newTimming = true;
    AjaxTimingitem.forEach(function (item, index) {
        if (item.link === settings.url) {
            AjaxTimingitem[index].time = Date.now();
            newTimming = false;
        }
    });
    if (newTimming) {
        var start = { link: settings.url, time: Date.now() }
        AjaxTimingitem.push(start);
    }

});
$(document).ajaxComplete(function (event, request, settings) {

    clearTimeout(timer);
    LoaderExit();
    var timeEnd = Date.now();
    AjaxTimingitem.forEach(function (item, index) {
        if (item.link === settings.url) {
            var timeLaps = (timeEnd - item.time) / parseFloat(1000);
            //if (timeLaps > 1) {
            //    console.warn("AjaxExcectime:" + timeLaps + "sec \t ||link:  " + item.link);
            //}

            AjaxTimingitem[index].time = Date.now();
        }
    });
    ConvertNumberToPersion
});
function AjaxFormSubmit($formEditor, $url, successFunction, showModalAlert) {
    successFunction = typeof successFunction !== 'undefined' ? successFunction : function () { };
    showModalAlert = typeof showModalAlert !== 'undefined' ? showModalAlert : true;
    //this detect $virtualDirectory is repeated in $url and correct it
    //if ($url.startsWith($ServerRoot) || $url.startsWith($ServerRoot, 1))
    $url = $url.toLowerCase().replace($ServerRoot.toLowerCase(), '');
    //if ($url.startsWith($VirtualDirectory) || $url.startsWith($VirtualDirectory, 1))
    if ($VirtualDirectory != '/') {
        $url = $url.toLowerCase().replace($VirtualDirectory.toLowerCase(), '');
    }

    var $destination = $ServerRoot + $VirtualDirectory + $url;
    var returnData = "";
    var data = $($formEditor).serialize().replace('<', '').replace('>', '');
    console.log(data);
    $.ajax({
        type: "Post",
        url: $destination,
        data: data,
        success: function (returnData) {
            if ($.isNumeric(returnData)) {
                if (parseInt(returnData) > 0) {
                    if (showModalAlert)
                        modalAlert("پیام سیستم", "عملیات با موفقیت انجام شد");

                    if (typeof successFunction === "string") {
                        if (successFunction != "") {
                            window[successFunction](returnData);
                        }
                    }
                    else
                        successFunction(returnData);
                } else {
                    if (showModalAlert)
                        modalAlert("پیام سیستم", "خطا در عملیات!!");
                }
            }
            else {
                if (showModalAlert)
                    modalAlert("پیام سیستم", returnData);
                if (typeof successFunction === "string") {
                    if (successFunction != "") {
                        window[successFunction](returnData);
                    }
                }
                else {
                    var str = jQuery.parseHTML(returnData);
                    successFunction(str);
                }
            }
            ConvertNumberToPersion();
        }
    });
    return returnData;
}
function AjaxModelSubmit($model, $url, successFunction, showModalAlert) {
    successFunction = typeof successFunction !== 'undefined' ? successFunction : function () { };
    showModalAlert = typeof showModalAlert !== 'undefined' ? showModalAlert : true;
    //this detect $virtualDirectory is repeated in $url and correct it
    //if ($url.startsWith($ServerRoot) || $url.startsWith($ServerRoot, 1))
    $url = $url.toLowerCase().replace($ServerRoot.toLowerCase(), '');
    //if ($url.startsWith($VirtualDirectory) || $url.startsWith($VirtualDirectory, 1))
    if ($VirtualDirectory != '/') {
        $url = $url.toLowerCase().replace($VirtualDirectory.toLowerCase(), '');
    }

    var $destination = $ServerRoot + $VirtualDirectory + $url;
    var returnData = "";
    var data =  $model;
    console.log(data);
    $.ajax({
        type: "Post",
        url: $destination,
        data: { "entity": data },
        success: function (returnData) {
            if ($.isNumeric(returnData)) {
                if (parseInt(returnData) > 0) {
                    if (showModalAlert)
                        modalAlert("پیام سیستم", "عملیات با موفقیت انجام شد");

                    if (typeof successFunction === "string") {
                        if (successFunction != "") {
                            window[successFunction](returnData);
                        }
                    }
                    else
                        successFunction(returnData);
                } else {
                    if (showModalAlert)
                        modalAlert("پیام سیستم", "خطا در عملیات!!");
                }
            }
            else {
                if (showModalAlert)
                    modalAlert("پیام سیستم", returnData);
                if (typeof successFunction === "string") {
                    if (successFunction != "") {
                        window[successFunction](returnData);
                    }
                }
                else {
                    var str = jQuery.parseHTML(returnData);
                    successFunction(str);
                }
            }
            ConvertNumberToPersion();
        }
    });
    return returnData;
}

//END AJAX Setup and Related functions////////////////////////////////////////////////

//DropDown Service Function/////////////////////////////////////////////////////////////

function FillItemsByTypeNameEn(itemNameEn, $HtmlEditor, $defaultValue, $Multiple, $hiddenValue) {
    // Filter Item In DropDown Person Role
    $hiddenValue = typeof $hiddenValue !== 'undefined' ? $hiddenValue : '';
    var _listhiddenArray = [];
    if ($hiddenValue != '') {
        _listhiddenArray = $hiddenValue.split(',');
    }
    $defaultValue = typeof $defaultValue !== 'undefined' ? $defaultValue : '';
    $.ajax({
        url: $ServerRoot + $VirtualDirectory + "/Public/Items/FillItemsByTypeNameEn?TypeNameEn=" + itemNameEn,
        success: function (jsonresult) {
            $($HtmlEditor).empty();
            if ($Multiple == false || $Multiple == '' || $Multiple == null) {
                var option = $('<option value="0"></option>').attr("value", "").text("-");
                $($HtmlEditor).append(option);
            }
            $.each(jsonresult, function (i) {
                if (jQuery.inArray(jsonresult[i].NameEn, _listhiddenArray) == -1) {
                    var option = $('<option></option>').attr("value", jsonresult[i].Id).text(jsonresult[i].NameFa);
                    $($HtmlEditor).append(option);
                }
            });
            $($HtmlEditor).val("");
            if (($defaultValue != '' || $defaultValue != 0) && ($Multiple == 'true' || $Multiple == true)) {
                $($HtmlEditor).val($defaultValue.split(',')).trigger('change');
            }
            else if (($defaultValue != '' || $defaultValue != 0) && ($Multiple == 'false' || $Multiple == '' || $Multiple == null || $Multiple == false)) {
                if ($defaultValue === "_lastInsertedValue") {
                    var maxId = getMax(jsonresult, "Id");
                    var $lastOptionValue = $($HtmlEditor + " option:last-child").val();
                    $($HtmlEditor).val(maxId.id).trigger('change');
                } else
                    $($HtmlEditor).val($defaultValue).trigger('change');
            }
        }
    });
}

function FillItemsByTypeId(itemTypeId, $HtmlEditor, $defaultValue, $Multiple, $hiddenValue) {
    // Filter Item In DropDown Person Role
    $hiddenValue = typeof $hiddenValue !== 'undefined' ? $hiddenValue : '';
    var _listhiddenArray = [];
    if ($hiddenValue != '') {
        _listhiddenArray = $hiddenValue.split(',');
    }
    $defaultValue = typeof $defaultValue !== 'undefined' ? $defaultValue : '';
    $.ajax({
        url: $ServerRoot + $VirtualDirectory + "/Public/Items/FillItemsByTypeId?TypeId=" + itemTypeId,
        success: function (jsonresult) {
            $($HtmlEditor).empty();
            if ($Multiple == false || $Multiple == '' || $Multiple == null) {
                var option = $('<option value="0"></option>').attr("value", "").text("-");
                $($HtmlEditor).append(option);
            }
            $.each(jsonresult, function (i) {
                if (jQuery.inArray(jsonresult[i].NameEn, _listhiddenArray) == -1) {
                    var option = $('<option></option>').attr("value", jsonresult[i].Id).text(jsonresult[i].NameFa);
                    $($HtmlEditor).append(option);
                }
            });
            $($HtmlEditor).val("");
            if (($defaultValue != '' || $defaultValue != 0) && ($Multiple == 'true' || $Multiple == true)) {
                $($HtmlEditor).val($defaultValue.split(',')).trigger('change');
            }
            else if (($defaultValue != '' || $defaultValue != 0) && ($Multiple == 'false' || $Multiple == '' || $Multiple == null || $Multiple == false)) {
                if ($defaultValue === "_lastInsertedValue") {
                    var maxId = getMax(jsonresult, "Id");
                    var $lastOptionValue = $($HtmlEditor + " option:last-child").val();
                    $($HtmlEditor).val(maxId.id).trigger('change');
                } else
                    $($HtmlEditor).val($defaultValue).trigger('change');
            }
        }
    });
}

function FillDropDownList(url, $HtmlEditor, $defaultValue, $Multiple, $hiddenValue) {
    // Filter Item In DropDown Person Role
    $hiddenValue = typeof $hiddenValue !== 'undefined' ? $hiddenValue : '';
    var _listhiddenArray = [];
    if ($hiddenValue != '') {
        _listhiddenArray = $hiddenValue.split(',');
    }
    $defaultValue = typeof $defaultValue !== 'undefined' ? $defaultValue : '';
    $.ajax({
        url: url,
        success: function (jsonresult) {
            $($HtmlEditor).empty();
            if ($Multiple == false || $Multiple == '' || $Multiple == null) {
                var option = $('<option value="0"></option>').attr("value", "").text("-");
                $($HtmlEditor).append(option);
            }
            $.each(jsonresult, function (i) {
                if (jQuery.inArray(jsonresult[i].NameEn, _listhiddenArray) == -1) {
                    var option = $('<option></option>').attr("value", jsonresult[i].Id).text(jsonresult[i].NameFa);
                    $($HtmlEditor).append(option);
                }
            });
            $($HtmlEditor).val("");
            if (($defaultValue != '' || $defaultValue != 0) && ($Multiple == 'true' || $Multiple == true)) {
                $($HtmlEditor).val($defaultValue.split(',')).trigger('change');
            }
            else if (($defaultValue != '' || $defaultValue != 0) && ($Multiple == 'false' || $Multiple == '' || $Multiple == null || $Multiple == false)) {
                if ($defaultValue === "_lastInsertedValue") {
                    var maxId = getMax(jsonresult, "Id");
                    var $lastOptionValue = $($HtmlEditor + " option:last-child").val();
                    $($HtmlEditor).val(maxId.id).trigger('change');
                } else
                    $($HtmlEditor).val($defaultValue).trigger('change');
            }
        }
    });
}

function FillSearchingDropDownList(url, $HtmlEditor, $name, $defaultValue, $Multiple, $hiddenValue) {
    // Filter Item In DropDown Person Role
    $hiddenValue = typeof $hiddenValue !== 'undefined' ? $hiddenValue : '';
    var _listhiddenArray = [];
    if ($hiddenValue != '') {
        _listhiddenArray = $hiddenValue.split(',');
    }
    $defaultValue = typeof $defaultValue !== 'undefined' ? $defaultValue : '';
    $.ajax({
        url: url + "?name=" + $name,
        success: function (jsonresult) {
            $($HtmlEditor).empty();
            if ($Multiple == false || $Multiple == '' || $Multiple == null) {
                var option = $('<option value="0"></option>').attr("value", "").text("-");
                $($HtmlEditor).append(option);
            }
            $.each(jsonresult, function (i) {
                if (jQuery.inArray(jsonresult[i].NameEn, _listhiddenArray) == -1) {
                    var option = $('<option></option>').attr("value", jsonresult[i].Id).text(jsonresult[i].NameFa);
                    $($HtmlEditor).append(option);
                }
            });
            $($HtmlEditor).val("");
            if (($defaultValue != '' || $defaultValue != 0) && ($Multiple == 'true' || $Multiple == true)) {
                $($HtmlEditor).val($defaultValue.split(',')).trigger('change');
            }
            else if (($defaultValue != '' || $defaultValue != 0) && ($Multiple == 'false' || $Multiple == '' || $Multiple == null || $Multiple == false)) {
                if ($defaultValue === "_lastInsertedValue") {
                    var maxId = getMax(jsonresult, "Id");
                    var $lastOptionValue = $($HtmlEditor + " option:last-child").val();
                    $($HtmlEditor).val(maxId.id).trigger('change');
                } else
                    $($HtmlEditor).val($defaultValue).trigger('change');
            }
        }
    });
}

