 /*!
 * Bancslink+ Client Libraries
 *
 *
 * Copyright 2014 Su Jia
 * Released under the MIT license:
 *
 * Date   : 2015/06/15
 * Author : Su Jia
 */

$(function () {
    // Tab Switch

    /*
    //设计案例hover效果
    $('.product-wrap .product li').hover(function () {
        $(this).css("border-color", "#ff6600");
        $(this).find('p > a').css('color', '#ff6600');
    }, function () {
        $(this).css("border-color", "#fafafa");
        $(this).find('p > a').css('color', '#666666');
    });
    */

    $("#bl_main_logo").click(function () {
        if ($.Client.Debug == true) {
            $.Client.Debug = false;
            $("#bl_debug").addClass("hidden");
        } else {
            $.Client.Debug = true;
            $("#bl_debug").removeClass("hidden");
        }
    })
});

function tabclick() {
    $('.bl_tab_list .bl_tab').on("click", function () {
        var liIndex = $('.bl_tab_list .bl_tab').index(this);
        $(this).addClass('bl_tab_selected').siblings().removeClass('bl_tab_selected');
        $(this).parent().parent().next().children().eq(liIndex).fadeIn(200).siblings().hide();

        //$('.product-wrap div.product').eq(liindex).fadeIn(150).siblings('div.product').hide();
        var liWidth = $('.bl_tab_list li').width();
        $('.bl_tab_list p').stop(false, true).animate({ 'left': liIndex * (liWidth + 4) + 'px' }, 500);
    });
}

function getLocaleTime() {
    $("#bl_clock").html(new Date().toLocaleString());
}

function ClickTxnTab() {
    $("#page_" + $.Client.ActivedPage).addClass("hidden").removeClass("fadeInUp");

    var PageId = $(this).attr("id").split("_")[2];
    $.Client.ActivedPage = PageId;
    $("#bl_txn_tabs .tab_selected").removeClass("tab_selected");
    $(this).addClass("tab_selected")
    
    $("#page_" + PageId).removeClass("hidden");
    $("#page_" + PageId).addClass("fadeIn animated").one('webkitAnimationEnd mozAnimationEnd MSAnimationEnd oanimationend animationend', function () {
        $(this).removeClass("fadeIn").removeClass("animated");
    });
}

function PrepareStatusBar() {
    var StatusBar = $("#page_" + $.Client.ActivedPage).find(".bl_status_bar");
    if (StatusBar.length == 0) {
        // Append New Status Bar
        if ($("#page_" + $.Client.ActivedPage).find(".bl_control_area").length > 0) {
            $("#page_" + $.Client.ActivedPage).find(".bl_control_area").prepend($.Client.StatusBar);
        } else $("#page_" + $.Client.ActivedPage).append($.Client.StatusBar)

    }
}

function OpenNewPage(IsDialog) {
    var TxnTab = $("#bl_txntab_new").clone();
    $(TxnTab).attr("id", "bl_txntab_" + $.Client.PageIndex).find("span").html("about:blank");
    $(TxnTab).find("img:eq(0)").attr("src", "Images/Icon_Transaction.fw.png")
    $(TxnTab).find("img:eq(1)").removeClass("hidden");
    (TxnTab).find("img:eq(1)").on("click", $.Client.PageIndex, ClosePage);
    $("#bl_txntab_new").before(TxnTab);
    $.Client.ActivedPage = $.Client.PageIndex;

    if (IsDialog == true) {
        var DialogContent = "<div id='page_" + $.Client.PageIndex + "' />";
        if ($(document.body).children(".blockMsg").length > 0) {
            $(document.body).children(".blockMsg").html(DialogContent).css("top", "100px");
        } else $.blockUI({ message: DialogContent, css: { top: '120px', cursor: 'auto', 'padding-top': '0px' } });
    } else {
        $("#bl_txn_tabs .tab_selected").removeClass("tab_selected");
        $(TxnTab).addClass("tab_selected")

        $("#page_" + $.Client.ActivedPage).addClass("hidden");
        $("#bl_transactions").append("<div class='bl_txn' id='page_" + $.Client.PageIndex + "'><h1>Do You Know?</h1><ul class='bl_tips_list'><li>If you want to open transaction, you can find the menu button above, or type txn code in search area</li><li>Tips : Press F1 after you open the transaction to get more help.</li></ul><p><br /></p></div>");
        $(TxnTab).on("click", ClickTxnTab)
    }
    
    $.Client.PageIndex++;

    /*
    var TxnTab = $("#bl_txntab_new").html();
    var PageId = $.Client.PageId;
    $(this).("<li id='bl_txntab_" + PageId + "'>" + NewTab + "</li>")
    */
}

function ClosePage(PageId) {
    if (PageId && PageId.data) { PageId = PageId.data };
    //if (!PageId) PageId = $(this).parent().attr("id").split("_")[2];
    $("#page_" + PageId).remove();
    $("#bl_txntab_" + PageId).remove();

    if ($(".tab_selected").length > 0) {
        var ActivedPage = $(".tab_selected").attr("id").split("_")[2];
        $.Client.ActivedPage = ActivedPage;
    }
}

//设置cookie
function setCookie(cname, cvalue, exdays) {
    var d = new Date();
    d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
    var expires = "expires=" + d.toUTCString();
    document.cookie = cname + "=" + cvalue + "; " + expires + "; path=/";
}
//获取cookie
function getCookie(cname) {
    var name = cname + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') c = c.substring(1);
        if (c.indexOf(name) != -1) return c.substring(name.length, c.length);
    }
    return "";
}
//清除cookie  
function clearCookie(name) {
    setCookie(name, "", -1);
}