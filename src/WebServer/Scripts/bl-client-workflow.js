// Workflow Object
function WorkflowData(CurrentWorkflowId) {
    this.Id = CurrentWorkflowId;
    this.Control = {
        FlowId: "StandardFlow",
        NodeId: "Start",
        WorkflowStack: [],
        NodeStack: [],
        Component: "",
        IsBackground: false,
        IsComplete: false,
        IsLastComponent: false,
        ProcTime: 0,
        SystemDate: new Date(),
        ErrDescription: "",
        ErrCode: 0,
        SystemId: null,
        Outcome: "",
        Notify: ""
    };
    this.UserInfo = new UserInfo();

    this.UserData = {
        ScreenData: {}
    };
}

function WorkflowDataObject(Source) {
    this.Source = Source;
    this.KeepData = null;
    this.Callback = null;
    this.FlowData = new WorkflowData();
    this.FlowData.UserInfo = $.Client.UserInfo;
    //this.FlowData.Id = $.Client.CurrentWorkflowId++;
    var CurrentDate = new Date();
    var Year = CurrentDate.getFullYear().toString();
    var Month = padLeft((CurrentDate.getMonth() + 1).toString(), 2);
    var Day = padLeft((CurrentDate.getDate()).toString(), 2);
    var Hour = padLeft((CurrentDate.getHours()).toString(), 2);
    var Minutes = padLeft((CurrentDate.getMinutes()).toString(), 2);
    var Seconds = padLeft((CurrentDate.getSeconds()).toString(), 2);

    this.FlowData.Id = Year + Month + Day + Hour + Minutes + Seconds + padLeft($.Client.CurrentWorkflowId++, 9);
}

function padLeft(str, length) {
    if (str.length >= length) {
        return str;
    } else {
        return padLeft("0" + str, length);
    }
}

$(document).ready(function () {
    if (typeof JSON == 'undefined') {
        $('head').append($("<script type='text/javascript' src='../Scripts/json2.js'>"));
    }
})

function UserInfo() {
    this.UserId = "";
    this.UserRole = 0;
    this.Token = "";
    this.Institution = 0;
    this.Terminal = 0;
}

(function ($) {
    $.extend({
        Workflow: {
            Test: function () {
                alert("Workflow Library Test OK");
            },
            OpenTransaction: function (PageCode, CustomerPage, HostData) {
                if (CustomerPage == true) {
                    $.Client.ActivedPage == 2;
                    $("#bl_txntab_2").click();
                } else  if ($.Client.ActivedPage == 1 || $.Client.ActivedPage == 2 || $("#page_" + $.Client.ActivedPage).length == 0) {
                    $("#bl_txntab_new").click();
                }

                // hidden menu
                var MenuContent = $("#bl_user_menu").html();
                $("#bl_user_menu").html("");
                $("#bl_user_menu").html(MenuContent);

                var WorkflowObject = new WorkflowDataObject(document);
                WorkflowObject.FlowData.Control.NodeId = "Start";
                WorkflowObject.FlowData.UserData.ScreenData.TxnCode = PageCode;

                if (HostData) {
                    WorkflowObject.FlowData.UserData.HostData = HostData;
                }

                $.Client.StartWorkflow(WorkflowObject);
            },
            UserAuthentication: function (Page) {
                var WorkflowObject = new WorkflowDataObject(Page);
                WorkflowObject.FlowData.Control.NodeId = "UserAuth";
                WorkflowObject.FlowData.UserData.HostData = {};
                WorkflowObject.FlowData.UserData.HostData.AuthButton = "OVERRIDE"

                $.Client.StartWorkflow(WorkflowObject, false);
            },
            CleanStart: function (FlowId) {
                var WorkflowObject = new WorkflowDataObject(document);
                if (FlowId) { WorkflowObject.FlowData.Control.FlowId = FlowId }
                WorkflowObject.FlowData.Control.NodeId = "CleanStart";
                $.Client.StartWorkflow(WorkflowObject);
            },
            // Create Source For Code
            // Option 0. Default, Display Busy Icon Inside Source
            HostQuery: function (WorkflowObject, TxnCode, FieldName, Option, Callback) {

                WorkflowObject.FlowData.Control.NodeId = "HostQuery";
                // Get Menu Xml (Internal Txn Code 999001)
                WorkflowObject.FlowData.UserData.ScreenData.TxnCode = TxnCode;
                WorkflowObject.FlowData.UserData.FieldName = "UserData/HostData/" + FieldName;

                if (WorkflowObject.Source && WorkflowObject.Source.nodeName == "LI") {
                    WorkflowObject.KeepData = $(WorkflowObject.Source).clone();
                }

                if (Option != null) {
                    switch (Option) {
                        case 0:
                            $(WorkflowObject.Source).html("<img class='loading' src='Images/Loading.gif' />");
                            break;
                        case 1:
                            $(WorkflowObject.Source).html("<option>Loading...</option>");
                            break;
                    }
                }

                WorkflowObject.Callback = Callback;

                if (Option == 3) {
                    $.Client.StartWorkflow(WorkflowObject, false);
                } else {
                    $.Client.StartWorkflow(WorkflowObject, true);
                }

            },
            PageQuery: function (WorkflowObject, TxnCode, PageId, ScreenData, Option, OptionContent, Callback) {
                if (Option != null) {
                    switch (Option) {
                        case 1:
                            var Content = "<div class='bl_details_aarow'><img src='../Images/Icon_Aarow_Up.fw.png'></div>";
                            var LoadingContent = "Loading...";
                            if (OptionContent) { LoadingContent = OptionContent }
                            Content += " <div class='bl_details_content'><img class='loading' src='Images/Loading.gif' /> " + LoadingContent + "</div>"
                            $(WorkflowObject.Source).html(Content);
                            $(WorkflowObject.Source).removeClass().addClass("bl_details fadeInUp animated");
                            break;
                        case 0:
                            var LoadingContent = "Loading...";
                            if (OptionContent) { LoadingContent = OptionContent }
                            var Content = "<img class='loading' src='Images/Loading.gif' /> " + LoadingContent;
                            $(WorkflowObject.Source).html(Content);
                            $(WorkflowObject.Source).removeClass("bl_details fadeInUp animated").addClass("bl_details fadeInUp animated");
                            break;
                    }
                }
                // Get Current Page Id
                var CurrentPage = (function () {
                    var Source = WorkflowObject.Source;
                    return $(Source).parentsUntil(".bl_txn")[0];
                })();
                WorkflowObject.FlowData.UserData.ScreenData = ScreenData;
                WorkflowObject.FlowData.UserData.ScreenData.NextPageId = PageId;

                if (CurrentPage) {
                    WorkflowObject.FlowData.UserData.ScreenData.PageId = $(CurrentPage).attr("pageid");
                }
                
                WorkflowObject.FlowData.UserData.ScreenData.TxnCode = TxnCode;
                WorkflowObject.Callback = Callback;

                if (TxnCode) {
                    WorkflowObject.FlowData.Control.NodeId = "PageQuery";
                } else WorkflowObject.FlowData.Control.NodeId = "GetPage";

                $.Client.StartWorkflow(WorkflowObject, true);
            }
        },
        Client: {
            // Start Workflow
            StartWorkflow: function (WorkflowObject, IsBackground) {
                //$.post("Services.ashx", json, function (data, status) {
                //    alert(status)
                //    if (status == "success") {
                //        var Data = JSON.parse(data);
                //        alert("Total Proc Time : " + Data.Control.ProcTime)
                //        if (Data.Control.ErrCode != 0)
                //            $.ClientSideComponent.BusinessExceptionHandler(Data);
                //        else
                //            $.ClientSideComponent[Data.Control.NodeId](Data);
                //    } else {
                //        alert("Proc Fail, Workflow Id " + FlowData.Id);
                //    }

                //});
                var FlowData = WorkflowObject.FlowData;
                FlowData.UserInfo = $.Client.CurrentUserInfo;
                //alert(json)
                $.Client.Workflows.put(FlowData.Id, WorkflowObject)
                //alert($.Client.Workflows.size());
                var Source = WorkflowObject.Source;
                //Post to Host
                if (IsBackground == undefined || IsBackground != true) {
                    $.blockUI();
                    WorkflowObject.FlowData.Control.IsBackground = false;
                } else WorkflowObject.FlowData.Control.IsBackground = true;

                $.Client.Send(WorkflowObject);
            },
            ContinueWorkflow: function (WorkflowObject) {

                if (WorkflowObject == null) {
                    var Page = "#" + $(event.srcElement).parents(".bl_txn")[0].id;
                    //var Page = element[element.length - 1].id;
                    var WorkflowObject = $.Client.Workflows.get($(Page).attr("workflow-id"))
                }

                var action = $(event.srcElement).attr("action");
                if (action) WorkflowObject.FlowData.Control.Outcome = action;

                if (WorkflowObject.FlowData.Control.IsBackground == false) {
                    if ($(document.body).children(".blockMsg").length > 0) {
                        $(document.body).children(".blockMsg").html($.blockUI.defaults.message).css("top", "250px");
                    } else $.blockUI();
                }
                $.Client.Send(WorkflowObject);
            },
            Send: function (WorkflowObject) {
                var json = JSON.stringify(WorkflowObject.FlowData);
                if ($.Client.Debug == true) $("#bl_debug_msg_list").append("<li><label>Send :</label>" + json + "</li>")
                var j = $.post("../Services.ashx", json, function (data) {
                    var Data = JSON.parse(data);
                    if ($.Client.Debug == true) $("#bl_debug_msg_list").append("<li><label>Receive :</label>" + data + "</li>")
                    //alert("Total Proc Time : " + Data.Control.ProcTime)
                    //alert(data)
                    WorkflowObject.FlowData = Data;
                    if (Data.Control.ErrCode != 0)
                        $.ClientSideComponent.BusinessExceptionHandler(WorkflowObject);
                    else {
                        var rtn;
                        try {
                            if (Data.Control.Notify != "") {
                                rtn = $.ClientSideComponent["Notify"](WorkflowObject);
                            } else {
                                WorkflowObject.FlowData.Control.IsComplete = true;
                                rtn = $.ClientSideComponent[Data.Control.Component](WorkflowObject);
                            }
                            // Stop Workflow When The Next Workflow Node is End
                            if (WorkflowObject.FlowData.Control.IsLastComponent == true) rtn = $.ClientSideComponent.End(WorkflowObject);
                            if (rtn == true) {
                                $.Client.Send(WorkflowObject);
                            } else {
                                if (!(Data.Control.Component == "Print" || Data.Control.Component == "BusinessExceptionHandler" || Data.Control.Component == "ShowDialogScreen") && WorkflowObject.FlowData.Control.IsBackground == false) $.unblockUI();
                            }

                        } catch (e) {
                            alert("Invalid Workflow Component : " + e)
                        }
                    }
                })
                //.done(function () {
                //    alert("second success");
                //})
                .fail(function () {
                    //debugger;
                    //alert("Workflow Id (" + FlowData.Id + ") Exception : " + j.statusText);
                })
                .always(function () {
                    /*
                    if (FlowData.Control.Notify != "") {
                        $.Client.Workflows.removeByKey(FlowData.Id);
                        //alert(WorkflowObject.FlowData.Control.IsBackground)
                        if (WorkflowObject.FlowData.Control.IsBackground == true) {
                            $.unblockUI();
                        }  
                    }
                    */
                });
            },
            SendTransaction: function (srcElement) {
                if (!srcElement) srcElement = event.srcElement;

                if ($(srcElement).validate() == false) {
                    return;
                }

                var t = $(srcElement).parents(".bl_txn");
                var Page = "#" + $(srcElement).parents(".bl_txn")[0].id;

                //var Page = element[element.length - 1].id;
                var WorkflowObject = $.Client.ScreenFlows.get($(Page).attr("id"))
                var ScreenData = WorkflowObject.FlowData.UserData.ScreenData;
                ScreenData.TxnCode = $(Page).attr("TxnCode");
                ScreenData.PageId = $(Page).attr("PageId");

                $.each($(Page + " :input"), function (idx, field) {
                    ScreenData[$(this).attr("name")] = $(this).val();
                });

                // Send the bind-item
                $.each($(Page + " [bind]"), function (idx, field) {
                    if ($(this).attr("submit") && $(this).attr("submit").toUpperCase() == "TRUE") {
                        if (this.nodeName == "TR") {
                            //ScreenData
                            var BindName = $(this).attr("bind");
                            if (!ScreenData[BindName]) ScreenData[BindName] = null;
                            ScreenData[BindName] = new Array();
                            ScreenData[BindName][ScreenData[BindName].length] = {};
                            var Element = ScreenData[BindName][ScreenData[BindName].length - 1];

                            $.each($(this).find("[bind-item]"), function (i, item) {
                                Element[$(this).attr("bind-item")] = $(this).html();
                            });
                        } else if (this.nodeName == "A") {
                            ScreenData[$(this).attr("bind")] = $(this).text();
                        }
                    }
                });
                var action = $(srcElement).attr("action");
                if (action) {
                    WorkflowObject.FlowData.Control.Outcome = action;
                }

                var PageData = $(Page).clone();
                $(PageData).find("[id]").attr("id", "");
                $(PageData).attr("id", "")
                $(PageData).find(".bl_control_area").remove();
                $(PageData).find("scripts").remove();
     
                zip.workerScriptsPath = "Scripts/"
                // use a BlobWriter to store the zip into a Blob object
                zip.createWriter(new zip.BlobWriter(), function (writer) {
                    // use a TextReader to read the String to add
                    writer.add("PageData.html", new zip.TextReader(PageData[0].outerHTML), function () {
                        // onsuccess callback

                        // close the zip writer
                        writer.close(function (blob) {
                            // blob contains the zip file as a Blob object
                            var reader = new FileReader();
                            reader.onloadend = function (evt) {
                                var array = new Uint8Array(evt.target.result, 0, evt.loaded);
                                var data = ""
                                $.each(array, function (idx, item) {
                                    var hex = item.toString(16).toUpperCase();
                                    if (hex.length == 1) hex = "0" + hex;
                                    data += hex;
                                })

                                ScreenData.PageData = data;
                                // Clone the screen flow to avoid change by response
                                var json = JSON.stringify(WorkflowObject.FlowData);
                                var data = JSON.parse(json);

                                var FlowObject = $.Client.Workflows.get($(Page).attr("workflow-id"))
                                FlowObject.Source = WorkflowObject.Source;
                                FlowObject.FlowData = data;

                                if ($(Page).hasClass("dialog_txn")) {
                                    // Is Dialog Transaction
                                    // Close Page.
                                    ClosePage($(Page).attr("id").split("_")[1]);
                                }

                                // Clean up Status Bar
                                $(Page).find(".bl_status_bar").addClass("hidden");

                                $.Client.ContinueWorkflow(FlowObject);
                            }
                            reader.readAsArrayBuffer(blob);
                        });
                    }, function (currentIndex, totalIndex) {
                        // onprogress callback
                    });
                }, function (error) {
                    // onerror callback
                    alert("error");
                });
   
            },
            Bind: function (Source, Attr, Value, Template) {
                if (!Value) { Value = "" }
                if (Attr) { $(Source).attr(Attr, Value); return; }
                if (!Source) return;

                if (Source.nodeName == 'INPUT' || Source.nodeName == 'SELECT') {
                    $(Source).val(Value);
                } else if (Source.nodeName == "IMG") {
                    $(Source).attr("src", Value);
                } else if (Source.nodeName == "LI" || Source.nodeName == "TR") {
                    var ul = $(Source).parent();
                    var template = $(Source).clone();
                    if (Template) { template = Template }
                    var IsTable = false;
                    if (Source.nodeName == "TR") IsTable = true;
                    $(Source).remove();
                    // Bind Items
                    $.each(Value, function (idx, item) {
                        $(template).find("[bind-item],[bind-attr]").attr("bind-item", function (idx2, ItemName) {
                            try {
                                if (ItemName) { $.Client.Bind(this, null, item[ItemName]); }
                            } catch (e) { }

                        }).attr("bind-attr", function (idx2, ItemName) {
                            try {
                                if (ItemName) {
                                    var items = ItemName.split(":")
                                    if (items.length == 2) {
                                        $.Client.Bind(this, items[1], item[items[0]]);
                                    }
                                }
                            } catch (e) { }
                        });
                        $(ul).append($(template).clone())
                    });
                    // Fix Header
                    if (IsTable && $(ul).parent().hasClass("fix_header")) {
                        var TableClone = $(ul).parent().parent().clone();
                        $(TableClone).find("[bind]").attr("bind", null);
                        $(TableClone).find("[bind-item]").attr("bind-item", null);
                        $(TableClone).css("position", "fixed");
                        $(TableClone).find("tr:gt(0)").css("visibility", "hidden");
                        $(TableClone).css("overflow-x", "hidden");
                        $(TableClone).css("overflow-y", "hidden");
                        $(TableClone).css("z-index", "2");
                        var Header = $(ul).find("tr:eq(0)")[0];
                        $(TableClone).css("width", ($(ul).parent().parent()[0].offsetWidth - 20) + "px");
                        $(TableClone).css("height", Header.offsetHeight.toString() + "px");
                        $(ul).find("tr:eq(0)").css("visibility", "hidden");
                        $(ul).parent().parent().before(TableClone);

                        $(ul).parent().parent().on("scroll", function () {
                            $(TableClone).scrollLeft($(this).scrollLeft());
                        })
                    }
                } else if (Source.nodeName == "DIV") {
                    $(Source).html(Value)
                } else $(Source).text(Value)
            },
            DataBind: function (Page, FlowData) {
                if (!FlowData.UserData.HostData) return;
                $(Page).find("[bind]").attr("bind", function (idx, BindName) {
                    $.Client.Bind(this, null, FlowData.UserData.HostData[BindName]);
                });
            },
            Workflows: new Map(),
            ScreenFlows: new Map(),
            OverrideFlows: new Map(),
            CurrentWorkflowId: 1,
            CurrentUserInfo: new UserInfo(),
            PageIndex: 3,
            ActivedPage: 1,
            StatusBar: "<div class='bl_status_bar'><div class='bl_status_img'><img src='../Images/Icon_Error.fw.png' /></div><div class='bl_status_info'><div class='bl_status_source'>Source : <a class='status_source'></a></div><div class='bl_status_errcode'>Err(<a class='status_code'></a>) : </div><div class='bl_status_message'><a class='status_message'></a></div></div></div>",
            Debug: false
        },

        ClientSideComponent: {
            SetupUserToken: function (WorkflowObject) {
                if (window.top) {
                    window.top.postMessage({ UserToken: WorkflowObject.FlowData.UserData.HostData.UserToken, UserId: WorkflowObject.FlowData.UserData.HostData.UserId }, "*")
                }
                return false;
            },
            FillData: function (WorkflowObject) {
                //$(WorkflowObject.Source).html(WorkflowObject.FlowData.UserData.Field);
                $.Client.Bind(WorkflowObject.Source, null, WorkflowObject.FlowData.UserData.Field, WorkflowObject.KeepData);

                //$('#bancslink_menu').fancytree();
                //alert(Source)
                if (WorkflowObject.Callback) {
                    WorkflowObject.Callback(WorkflowObject);
                }
                //alert("Fill Data");
                return true;
            },
            PageQueryErrorHandler: function (WorkflowObject) {
                // Display Error Message in Queried Page
                var HostData = WorkflowObject.FlowData.UserData.HostData;
                var ErrMessage = HostData.ErrCode + " : " + HostData.ErrDescription

                var Source = $(WorkflowObject.Source).find(".bl_details_content");
                if (Source.length > 0) {
                    $(Source).html(ErrMessage);
                } else {
                    $(WorkflowObject.Source).html(ErrMessage);
                }

            },
            FillPageData: function (WorkflowObject) {
                var Target;

                if ($(WorkflowObject.Source).find(".bl_details_content").length > 0) {
                    Target = $(WorkflowObject.Source).find(".bl_details_content")[0];
                } else if ($(WorkflowObject.Source).find(".bl_details").length > 0) {
                    Target = $(WorkflowObject.Source).find(".bl_details")[0];
                } else Target = WorkflowObject.Source;

                if (!Target) return true;

                $(Target).html(WorkflowObject.FlowData.UserData.PageData.Content);
                // Bind Data With Page
                $.Client.DataBind(Target, WorkflowObject.FlowData);
                // Execute Ready Function
                var action = "Action" + WorkflowObject.FlowData.UserData.PageData.PageId;
                if (typeof window[action] === 'function') {
                    var p = new window[action]();
                    try {
                        p.Ready($(Target)[0]);
                    } catch (ex) { }
                }


                // Check Transaction Properties, If This is a transaction page(with bl_txn class), then keep the screen data
                $.Client.PageIndex++;
                var TxnPage = $(Target).find(".bl_txn");
                if (TxnPage.length > 0) {
                    $(TxnPage[0]).attr("id", "sub_page_" + $.Client.PageIndex);
                    $(TxnPage[0]).attr("workflow-id", WorkflowObject.FlowData.Id);

                    // Clone Existing Workflow Object
                    var ScreenFlowObject = new WorkflowDataObject();
                    // Make a copy to screen flows
                    ScreenFlowObject.Source = TxnPage[0];
                    var json = JSON.stringify(WorkflowObject.FlowData);
                    var data = JSON.parse(json);
                    ScreenFlowObject.FlowData = data;

                    $.Client.ScreenFlows.put($(TxnPage[0]).attr("id"), ScreenFlowObject);
                }

                $(Target).removeClass("fadeInDown animated").addClass("fadeInDown animated slow").one('webkitAnimationEnd mozAnimationEnd MSAnimationEnd oanimationend animationend', function () {
                    $(this).removeClass("fadeInDown animated");
                });

                // Do Callback
                if (WorkflowObject.Callback) {
                    WorkflowObject.Callback(WorkflowObject);
                }

                //alert("Fill Data");
                return true;
            },

            ShowDialogScreen: function (WorkflowObject) {
                return $.ClientSideComponent.ShowScreen(WorkflowObject, true)
                //var PageData = WorkflowObject.FlowData.UserData.PageData;
                //$.Client.PageIndex++;
                //$.blockUI({ message: "<div id='page_" + $.Client.PageIndex + "'><button class='close_button' type='button'><img src='Images/Close_Button.fw.png' /></button><div class='user_authentication'></div></div>", css: { top: '120px', cursor: 'auto', 'padding-top': '0px' } });
                //WorkflowObject.Source = $(".user_authentication")[0];
                //var Page = "#page_" + $.Client.PageIndex;

                //$(Page).removeClass().addClass("bl_txn");
                //$(Page).attr("workflow-id", WorkflowObject.FlowData.Id)
                //$(Page).attr("PageId", PageData.PageId)
                //$(Page).attr("TxnCode", PageData.TxnCode)

                //var ScreenFlowObject = new WorkflowDataObject();
                //// Make a copy to screen flows
                //ScreenFlowObject.Source = $(Page)[0];
                //var json = JSON.stringify(WorkflowObject.FlowData);
                //var data = JSON.parse(json);
                //ScreenFlowObject.FlowData = data;

                //$.Client.ScreenFlows.put(ScreenFlowObject.FlowData.Id, ScreenFlowObject);

                //$.ClientSideComponent.FillPageData(WorkflowObject);
                //if (PageData.OnReady) {
                //    eval(PageData.OnReady);
                //}

                //$(WorkflowObject.Source).find("input")[0].focus();


                //$(".blockUI .close_button").on("click", function () {
                //    $.unblockUI();
                //})
            },
            OK: function (WorkflowObject) {
                PrepareStatusBar();

                StatusBar = $("#page_" + $.Client.ActivedPage).find(".bl_status_bar")[0];
                $(StatusBar).removeClass("hidden");
                $(StatusBar).find(".bl_status_img img").attr("src", "../Images/Icon_OK.fw.png");
                $(StatusBar).find(".status_source").html(WorkflowObject.FlowData.Control.SystemId);
                $(StatusBar).find(".status_code").html(WorkflowObject.FlowData.UserData.HostData.OKCode);
                $(StatusBar).find(".status_message").html(WorkflowObject.FlowData.UserData.HostData.OKMessage);

                return true;
            },
            Print: function (WorkflowObject) {
                var HostData = WorkflowObject.FlowData.UserData.HostData;
                var Message = ""
                Message="<h1>Printing Documents</h1>"
                for (var p in HostData) {
                    Message += p + " : " + HostData[p] + "<br />"
                }

                $(".blockMsg").html(Message).css("top", "50px");
                
                $(".blockMsg").on("click", function () { $.unblockUI(); })

                return false;
            },
            BusinessExceptionHandler: function (WorkflowObject) {
                var FlowData = WorkflowObject.FlowData;
                // alert(JSON.stringify(WorkflowObject.FlowData))
                if (FlowData.Control.IsBackground == false) {
                    var ErrCode = FlowData.Control.ErrCode;
                    var ErrDescription = FlowData.Control.ErrDescription;
                    if (FlowData.Control.ErrCode == 0) {
                        ErrCode = FlowData.UserData.HostData.ErrCode;
                        ErrDescription = FlowData.UserData.HostData.ErrDescription;
                    }

                    var BlockedErrorMessage = "<div class='blockError'><div class='blockErrorImage'><img src='Images/Info.fw.png'></div>"
                    BlockedErrorMessage += "<div class='blockErrorMessage'><h1>哇！糟糕，系统粗现了一些问题</h1><br />错误来源：" + FlowData.Control.SystemId + "<br />错误原因：" + ErrCode + "（" + ErrDescription + "）<br /></div></div>"

                    $(".blockPage").html(BlockedErrorMessage).addClass("fadeInUp animated fast").one('webkitAnimationEnd mozAnimationEnd MSAnimationEnd oanimationend animationend', function () {
                        $(this).removeClass("fadeInUp animated fast");
                    });

                    PrepareStatusBar();

                    StatusBar = $("#page_" + $.Client.ActivedPage).find(".bl_status_bar")[0];
                    $(StatusBar).removeClass("hidden");
                    $(StatusBar).find(".status_source").html(FlowData.Control.SystemId);
                    $(StatusBar).find(".status_code").html(ErrCode);
                    $(StatusBar).find(".status_message").html(ErrDescription);

                    //try {
                    var a = $(".blockPage .blockErrorImage")
                    $(".blockPage .blockErrorImage")[0].focus();
                    //} catch (ex) { }


                    $(".blockUI").bind("keydown click", function () {
                        if ((event.which == 27 || event.which == 13) || event.type == "click") {
                            $.unblockUI();
                            $(".blockUI").unbind("click keydown");

                            var inputs = $("#page_" + $.Client.ActivedPage).find("input");
                            var focusd = false;
                            $.each(inputs, function (idx, input) {
                                if ($(input).attr("err")) {
                                    var errCodes = $(input).attr("err").split(" ");
                                    for (var i = 0; i <= errCodes.length - 1; i++) {
                                        if (FlowData.Control.ErrCode == errCodes[i] || FlowData.UserData.HostData.ErrCode == errCodes[i]) {
                                            $(input)[0].focus();
                                            focusd = true;
                                            $(input).addClass("error");
                                        }
                                    }
                                }
                            });

                            if (focusd == false) {
                                try {
                                    $("#page_" + $.Client.ActivedPage).find(":input")[0].focus();
                                } catch (ex) { }
                            }
                        }
                    });

                } else {
                    alert("Error(" + FlowData.Control.ErrCode + ") : " + FlowData.Control.ErrDescription);
                }

                return false;
            },
            ClientInitial: function (WorkflowObject) {
                $("#bl_txntab_1").removeClass().addClass("tab_selected");
                $("#bl_txntab_2").removeClass();
                $("#bl_transactions").html("");
                $("#bl_transactions").append("<div id='page_1' class='bl_txn'></div>");
                $("#bl_transactions").append("<div id='page_2' class='bl_txn hidden'></div>");
                // Save txn new tabs
                var NewTab = $("#bl_txntab_new").clone();
                // remove all txn tabs
                $("#bl_txn_tab ul li").remove();
                $("#bl_txn_tab ul").append(NewTab);
                $.Client.ActivedPage = 1;

                $("#bl_txn_tabs ul li").on("click", ClickTxnTab);

                $("#bl_txntab_new").on("click", OpenNewPage);
                //$("<li id='bl_txntab_new'></li>").

                WorkflowObject.FlowData.Control.Notify = "Initilise Client";
                $.ClientSideComponent.Notify(WorkflowObject);
                return true;
            },
            ShowScreen: function (WorkflowObject, IsDialog) {
                WorkflowObject.FlowData.Control.Notify = "Show Screen";
                $.ClientSideComponent.Notify(WorkflowObject);

                var PageData = WorkflowObject.FlowData.UserData.PageData;
                if (IsDialog == true) {
                    OpenNewPage(true);
                }
                var PageId = $.Client.ActivedPage;
                var Page = "#page_" + PageId;

                $(Page).html("");
                $(Page).removeClass().addClass("bl_txn");
                $(Page).attr("workflow-id", WorkflowObject.FlowData.Id)
                $(Page).attr("PageId", PageData.PageId)
                $(Page).attr("TxnCode", PageData.TxnCode)

                if (IsDialog == true) $(Page).addClass("dialog_txn")

                if ($.Client.ActivedPage > 2) {
                    $("#bl_txntab_" + $.Client.ActivedPage).find("span").html(PageData.PageId + " : " + PageData.Title)
                }

                if (PageData.Content && PageData.Content.replace(/(^s*)|(s*$)/g, "").length != 0) {
                    // Custimized Mode
                    $(Page).html(PageData.Content)
                    $.each(PageData.Fields, function (idx, Field) {
                        if (Field.ErrorCode) {
                            $(Page).find("[name='" + Field.Name + "']").attr("err", Field.ErrorCode);
                            //alert($(Page).find("[name]").length)
                        }
                    });

                    if (IsDialog == true) {
                        var t = $(Page)[0];
                        var width = 0;
                        $.each($(Page).children(), function (idx, item) {
                            var totalWidth = item.offsetWidth;
                            if (item.offsetLeft) totalWidth += item.offsetLeft;
                            if (item.offsetRight) totalWidth += item.offsetRight;
                            if (totalWidth > width) width = totalWidth;
                        });
                        $(Page).parent().css("width", width)
                    }
                } else {
                    // Standard Mode;
                    $(Page).html("").append("<h1>" + PageData.PageId + " : " + PageData.Title + "</h1>")
                    $(Page).append("<form><table class='txn_table'></table></form>");
                    $.each(PageData.Fields, function (idx, Field) {
                        var FieldId = "page_" + $.Client.ActivedPage + "_" + idx;

                        if (!Field.Name || Field.Name.replace(/(^s*)|(s*$)/g, "").length == 0) {
                            $(Page + " table").append("<tr><td></td><td");
                            return;
                        }

                        var FieldContent = "<tr><td>" + Field.Description + "</td><td>"
                        var ErrorMapping = ""
                        if (Field.ErrorCode) {
                            ErrorMapping = " err='" + Field.ErrorCode + "' "
                        }

                        if (Field.InputType == 0) {
                            $(Page + " table").append(FieldContent + "<input id='" + FieldId + "' name='" + Field.Name + "' " + ErrorMapping + " /></td></tr>")
                        } else {
                            // Combobox
                            FieldContent += "<div class='sel_wrap'><label>请选择</label>"
                            $(Page + " table").append(FieldContent + "<select class='select' id='" + FieldId + "' name='" + Field.Name + "' " + ErrorMapping + "><option>AX : Other Income</option><option>CU : Salary</option></select></div></td></tr>")
                        }

                        if (Field.IsMandatory == true) $("#" + FieldId).addClass("required");
                        if (Field.Width != 0) { $("#" + FieldId).css({ 'width': Field.Width + 'px' }) }
                        if (Field.Formatter) { $("#" + FieldId).attr("Formatter", Field.Formatter) }

                        if (Field.FieldType && Field.FieldType.replace(/(^s*)|(s*$)/g, "").length != 0) {
                            var FieldTypes = Field.FieldType.split(" ")
                            $.each(FieldTypes, function (idx, value) {
                                $("#" + FieldId).addClass(value);

                                if (value == "account") {
                                    var required = "";
                                    if (Field.IsMandatory == true) required = "required";
                                    $("<div style='width: auto' class='following sel_wrap'><label>请选择</label><select class='select currency " + required + "' name='Currency'><option value='TWD'>TWD:Taiwan Dollar</option><option value='JPY'>JPY:Japanese Yuan</option></select></div><div class='bl_details' />").insertAfter("#" + FieldId);
                                }
                            });
                        }
                    })
                }

                if (!(PageData.TransmitButton == undefined && PageData.CloseButton == undefined)) {
                    var CloseButton = "<button type='button' class='bl_close_button'>Close(<U>C</U>)</button>";
                    var TransmitButton = "<button type='button' class='bl_transmit_button'>Transmit(<U>T</U>)</button>"

                    var ControlArea = "<div class='bl_control_area'><div>";
                    if (PageData.CloseButton == true) ControlArea += CloseButton;
                    if (PageData.TransmitButton == true) ControlArea += TransmitButton;
                    ControlArea += "</div></div>"

                    if (PageData.CloseButton == true || PageData.TransmitButton == true)  $(Page).find("form").append(ControlArea);
                    if ($.isMobile.any()) $(Page).find(".bl_control_area").addClass("mobile");
                }

                if ((PageData.CloseButton == undefined || PageData.CloseButton == false) && (PageData.TransmitButton == undefined || PageData.TransmitButton == false)) {
                    $(Page).addClass("no-padding")
                }

                if ((PageData.CloseButton == undefined || PageData.CloseButton == false) && IsDialog == true) {
                    // Add Close Button for Dialog Screen
                    var CloseButton = "<button class='close_button' type='button'><img src='Images/Close_Button.fw.png'></button>"
                    $(Page).prepend(CloseButton);
                }

                //// Get Account Infomation
                //if ((e && e.type == "blur") && $(source).hasClass("account") && $(source).val() && $(source).parent().find(".bl_details").length > 0) {
                //    var Target = $(source).parent().find(".bl_details")[0];
                //    $(source).on("change", function () {
                //        $(Target).addClass("hidden");
                //    })
                //    var WorkflowObject = new WorkflowDataObject(Target);
                //    // WorkflowObject.FlowData.UserData.AccoutNumber = $(source).val();
                //    var ScreenData = {};
                //    ScreenData[$(source).attr("name")] = $(source).val();
                //    $.Workflow.PageQuery(WorkflowObject, '000400', 'AccountSummary', ScreenData, 1, 'Loading Account Information');
                //}

                $(Page).find(".account").on("change", function (e) {
                    if ($(this).val().length > 0) {     
                        if ($.validator.input_validation(e, this) == true) {
                            var Target = $(this).parent().find(".bl_details")[0];
                            var WorkflowObject = new WorkflowDataObject(Target);
                            var ScreenData = {};
                            ScreenData[$(this).attr("name")] = $(this).val();
                            $.Workflow.PageQuery(WorkflowObject, '000400', 'AccountSummary', ScreenData, 1, 'Loading Account Information');
                        }
                    } else {
                        $(this).parent().find(".bl_details").addClass("hidden");
                    }
                })


                // Close Page
                $(Page).find(".close_button").on("click", function () {
                    ClosePage(PageId);
                    $.unblockUI();
                })

                $(Page).find(".bl_transmit_button").on("click", function () {
                    $(Page).find("")
                    $.Client.SendTransaction();
                })

                // Select Change
                $(Page).find(".sel_wrap").on("change", function () {
                    var o;
                    var opt = $(this).find('option');
                    opt.each(function (i) {
                        if (opt[i].selected == true) {
                            o = opt[i].innerHTML;
                        }
                    })
                    $(this).find('label').html(o);
                }).trigger('change');

                $(Page).find("select").on("focus", function () {
                    if ($(this).parent().hasClass("sel_wrap")) {
                        $(this).parent().addClass("sel_wrap_focus");
                    }
                });

                $(Page).find("select").on("blur", function () {
                    if ($(this).parent().hasClass("sel_wrap")) {
                        $(this).parent().removeClass("sel_wrap_focus");
                    }
                });

                // Reset Page Data
                WorkflowObject.FlowData.UserData.PageData = {};
                $(Page).val_bind();
                var action = "Action" + PageData.PageId;
                // Triggle PageInit Function
                if (typeof window[action] === 'function') {
                    var p = new window[action]();
                    try {
                        p.PageInit($(Page)[0], WorkflowObject);
                    } catch (ex) { }
                }

                // Fill Data
                $.Client.DataBind($(Page), WorkflowObject.FlowData);

                // auto-focus
                $(Page).addClass("fadeIn animated");
                // Abanden Later
                if (PageData.OnReady) {
                    eval(PageData.OnReady)
                }

                if (typeof window[action] === 'function') {
                    var p = new window[action]();
                    try {
                        p.Ready($(Page)[0], WorkflowObject);
                    } catch (ex) { }
                }

                try {
                    $(Page).find("input")[0].focus();
                } catch (ex) {

                }

                // Keep The Source in Original Screen.
                WorkflowObject.Source = $(Page)[0];
                // Clean up Screen Data
                WorkflowObject.FlowData.UserData.ScreenData = {};
                // Save Workflow in Temprary Screen Flows

                var ScreenFlowObject = new WorkflowDataObject();
                // Make a copy to screen flows
                ScreenFlowObject.Source = $(Page)[0];
                var json = JSON.stringify(WorkflowObject.FlowData);
                var data = JSON.parse(json);
                ScreenFlowObject.FlowData = data;

                $.Client.ScreenFlows.put($(Page).attr("id"), ScreenFlowObject);
                return false;
            },
            OverrideRequired: function (WorkflowObject) {
                var Page = WorkflowObject.Source;
                // Create a new Workflow For Superisor Override
                var OverrideFlowObject = new WorkflowDataObject();

                $(Page).find(":input").attr("disabled", "true")
                $(Page).prepend("<div class='override_request'><div>")
                //$(Page).find(".override_request").nextAll().block({ message: null, overlayCSS: { 'background-color': 'white', 'opacity': 0.1 } });
                //$(Page).find(".bl_control_area").block({ message: null, overlayCSS: { 'background-color': 'white', 'opacity': 0.1 } });

                OverrideFlowObject.Source = $(Page).find(".override_request")[0];
                var json = JSON.stringify(WorkflowObject.FlowData);
                var data = JSON.parse(json);
                OverrideFlowObject.FlowData = data;
                // Fill Data
                $.ClientSideComponent.FillPageData(OverrideFlowObject);

                $(OverrideFlowObject.Source).find(".close_button").on("click", function () {
                    //$(Page).find(".override_request").nextAll().unblock();
                    //$(Page).find(".bl_control_area").unblock();
                    $(Page).find(":input").attr("disabled", null)
                    $(Page).find(".bl_status_bar").addClass("hidden");

                    $(Page).find(".override_request").toggle("slow", function () {
                        $(this).remove();
                    });
                })
                return false;
            },
            CloseOverridePage: function (WorkflowObject) {
                var PageId = "#page_" + $.Client.ActivedPage;
                $(PageId).find(".override_request .close_button").click();

                WorkflowObject.Source = $(PageId)[0];
                return true;
            },
            SystemInitial: function (WorkflowObject) {
                WorkflowObject.FlowData.Control.Notify = "Initilise User Interface";
                $.ClientSideComponent.Notify(WorkflowObject);

                $("header").addClass("bl_header_login animated");
                $("#bl_nav").removeClass().addClass("fadeIn animated");
                $("footer").removeClass().addClass("fadeOut animated hidden");
                $("#bl_transactions").addClass("bl_transactions_login");
                //if (!isMobile.any()) {
                    $("#bl_notification").removeClass().addClass("fadeIn animated");
                //} else {
                    //$("#bl_transactions").addClass("mobile");
                   // $("#bl_nav").addClass("mobile");
                //}
                $("#page_1").html("")

                var HostData = WorkflowObject.FlowData.UserData.HostData;
                // Initilise User Info
                var User = new UserInfo()
                User.UserId = HostData.AuthorizedUser;
                User.Terminal = 3;
                User.Institution = HostData.Branch;

                $.Client.CurrentUserInfo = User;

                // Get User Menu In Background
                var WorkflowObject = new WorkflowDataObject($("#bl_user_menu")[0]);
                $.Workflow.HostQuery(WorkflowObject, "999995", "UserMenu", 0);

                var WorkflowObject1 = new WorkflowDataObject($("#bl_notify_items ul li")[0]);
                // WorkflowObject.FlowData.UserData.AccoutNumber = $(source).val();
                $.Workflow.HostQuery(WorkflowObject1, '999997', 'Notifications', 0, function () {
                    $("#bl_notify_items").find("[category]").attr("category", function (idx, category) {
                        var src = ""
                        if (category == 1) {
                            src = "Images/Icon_Sup.png";
                        } else if (category == 2) {
                            src = "Images/Icon_Broadcast.png"
                        } else src = "Icon_Broken.png";

                        $(this).find("img").attr("src", src)

                        $(this).on("click", function () {
                            $(this).next().slideToggle("slow");

                            if ($(this).hasClass("expended")) {
                                $(this).removeClass("expended");
                            } else {
                                $(this).addClass("expended");
                            }

                            if ($(this).next().hasClass("hidden")) {
                                var WorkflowObject2 = new WorkflowDataObject($(this).next().find("li")[0]);
                                WorkflowObject2.FlowData.UserData.ScreenData.NotificationCategory = category;
                                $.Workflow.HostQuery(WorkflowObject2, '999996', 'NotificationList', 0)
                            }

                            $(this).next().removeClass("hidden")
                        })
                    })
                });

                return true;
            },
            Notify: function (WorkflowObject) {
                // Only notify in front mode;
                if (WorkflowObject.FlowData.Control.IsBackground == true) return;

                if ($("#block-message").html() != "") {
                    $("#block-message").addClass("fadeOutUp animated fast").one('webkitAnimationEnd mozAnimationEnd MSAnimationEnd oanimationend animationend', function () {
                        $(this).removeClass().html("");
                        $.ClientSideComponent.Notify(WorkflowObject);
                    });
                } else {
                    var messages = WorkflowObject.FlowData.Control.Notify.split("|");
                    var msg;
                    if (messages.length > 1) {
                        msg = "<span>" + messages[1] + "&nbsp;&nbsp;&nbsp;</span>"
                        msg += "<span class='bl_small_date'>(" + messages[0] + ")</span>"
                    } else msg = messages[0]

                    $("#block-message").html("<b></b>" + msg);
                    $("#block-message").addClass("fadeInUp animated fast").one('webkitAnimationEnd mozAnimationEnd MSAnimationEnd oanimationend animationend', function () {
                        $(this).removeClass();
                    });
                }

                return true;
            },
            End: function (WorkflowObject) {
                if (!WorkflowObject.Source) {
                    //Source has been deleted
                    $.Client.Workflows.removeByKey(WorkflowObject.FlowData.Id);
                }
                //
                return false;
            }
        }
    })
}(jQuery));

var isMobile = {
    android: function () {
        return navigator.userAgent.match(/Android/i) ? true : false;
    },
    blackBerry: function () {
        return navigator.userAgent.match(/BlackBerry/i) ? true : false;
    },
    iOS: function () {
        return navigator.userAgent.match(/iPhone|iPad|iPod/i) ? true : false;
    },
    iPad: function () {
        return navigator.userAgent.match(/iPad/i) ? true : false;
    },
    windows: function () {
        return navigator.userAgent.match(/IEMobile/i) ? true : false;
    },
    any: function () {
        return (isMobile.android() || isMobile.blackBerry() || isMobile.iOS() || isMobile.windows());
    }
};