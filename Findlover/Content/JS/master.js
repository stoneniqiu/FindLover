$(function () {
    //-------- 消息提示：------------------------------------------------
    //总的消息，私信，赞，访客，喜欢，系统通知。 
    function checkforNewMessage() {
        $.post("/Base/GetUnReadMessageSum", function (data) {
            if (data.Total != 0) {
                $(".msgTag").html(data.Total).show();
                data.Private != 0 ? $(".msgTag1:eq(0)").html(data.Private).show() : $(".msgTag1:eq(0)").hide();
                data.Praises != 0 ? $(".msgTag1:eq(1)").html(data.Praises).show() : $(".msgTag1:eq(1)").hide();
                data.Visitor != 0 ? $(".msgTag1:eq(2)").html(data.Visitor).show() : $(".msgTag1:eq(2)").hide();
                data.LoveMe != 0 ? $(".msgTag1:eq(3)").html(data.LoveMe).show() : $(".msgTag1:eq(3)").hide();
                data.System != 0 ? $(".msgTag1:eq(4)").html(data.System).show() : $(".msgTag1:eq(4)").hide();
            } else {
                $(".msgTag,.msgTag1").hide();
            }
        });
    }

    
    checkforNewMessage();
    var lun = setInterval(checkforNewMessage, 30000);//30秒钟循环一次

    // 管理员出现 显示后台管理条
    $.post("/Admin/IsAdmin", function(data) {
        if (data == 1) {
            $("#adminli").show();
        }
    });
    $.post("/Base/CheckValid", function (data) {
        if (data == -1) {
            $("#loginli").show();
        }
    });



    //私信处理-------------------chat------------------------------------
    $(".chat").each(function (n) {
        $(this).click(function (e) {
            e.stopPropagation();
            $.post("/Base/CheckUser", {}, function (data) {
                if (data.IsOk) {
                    var id = $(".chat a").eq(n).attr("data-id");
                    var name = $(".chat a").eq(n).attr("data-name");
                    var msgtype = $(".chat a").eq(n).attr("data-msgType");
                    var statetype = $(".chat a").eq(n).attr("data-stateType");
                    var stateid = $(".chat a").eq(n).attr("data-relateId");
                    $(".toname").html(name).attr("data-msgType", msgtype).attr("data-stateType", statetype).attr("data-relateId", stateid);
                    $(".toid").html(id);
                    $('#msg_box').modal('show');
                } else {
                    if (!data.IsInfoOk) {
                        alert("你的资料未通过审核不能使用次功能，使用正确的邮箱或者工号,需要修改请私信管理员");
                        return;
                    }
                    if (!data.IsPersentOk) {
                        alert("你的资料完整度小于30%,暂时不能使用私信功能,请先在个人主页里完善你的资料");
                        return;
                    }
                    if (!data.IsImgOk) {
                        alert("你的头像未通过审核,需要上传个人真实照片后才可使用此功能");
                        return;
                    }
                    if (!data.IsLogon) {
                        alert("需要登录");
                        return;
                    }
                }
            });
        });
    });
    //统计独白的字数
    var $tex = $(".message1 span:eq(0) i");
    var $but = $(".msg_content");
    var ie = jQuery.support.htmlSerialize;
    var str = 0;
    var abcnum = 0;
    var maxNum = 800;
    var texts = 0;
    var total;
    var enable = false;
    if (ie) {
        $but[0].oninput = changeNum;
    } else {
        $but[0].onpropertychange = changeNum;
    }
    function changeNum() {
        //汉字的个数
        str = ($but.val().replace(/\w/g, "")).length;
        //非汉字的个数
        abcnum = $but.val().length - str;
        total = str * 2 + abcnum;
        texts = Math.ceil((maxNum - (total)) / 2);
        if (total > maxNum) {
            $tex.css("color", 'red');
            enable = false;
        } else {
            $tex.css("color", 'black');
            enable = true;
        }
        if (str == 0) {
            enable = false;//控制能否提交的一个条件 就是限制字数
        }
        $tex.html(texts);
    }

    //弹出对话框
    $(".modal-footer>.btn").click(function () {
        var toid = $(".toid").html();
        var msgtype = parseInt($(".toname").attr("data-msgType"));
        var statetype = parseInt($(".toname").attr("data-stateType"));
        var stateid = parseInt($(".toname").attr("data-relateId"));
        var content = $(".msg_content").val();
        if ($.trim(content) == "") {
            mm("亲,你还没有输入哦");
            return;
        }
        if (enable) {
            //MsgType StateId Statetype 
            $.post("/Message/SendMessage", { toid: toid, content: content, type: msgtype, statetype: statetype, stateid: stateid }, function (data) {
                $(".message1 .error").fadeIn(1000).css("color", '#008080').html("发送成功");
                var ss = setTimeout(function () {
                    $(".message1 .error").fadeOut(1000, function () {
                        $('#msg_box').modal('hide');
                    });
                }, 1000);
                $(".msg_content").val("");
                $tex.html(400);
            });
        } else {
            mm("亲,一次一次慢慢说,字数不要太多");
        }

        function mm(txt, color) {
            $(".message1 .error").fadeIn(1000).html(txt);
            var st1 = setTimeout(function () {
                $(".message1 .error").fadeOut(1000);
            }, 2000);
        }
    });
    //举报框----------------------report--------------------------------------------
    $(".report").each(function () {
        $(this).click(function () {
            var id = $(this).attr("data-userId");
            var name = $(this).attr("data-userName");
            var sextag = $(this).attr("data-sextag");//对方性别
            var type = $(this).attr("data-reportType");
            var relateId = $(this).attr("data-relateId");
            var content = $(this).attr("data-content");

            $("#reportedname").html(name).attr("data-id", id);//被举报人id
            $(".reportinfo span").html(sextag);
            $("#reportName").html(name);
            $("#reporttype").val(type);//举报类型
            $("#reporttype").attr("data-relateId", relateId);
            $("#reporttype").attr("data-content", content);

            //值是举报或者拉黑这个人 初始化
            $(".divrow:eq(1)").show();
            $("#reportcontent").hide();
            $(".reportmessage").html();
            $("#reportcontent").html(content);
            //不同类型，不同对话
            switch (type) {
                case "personal":
                    $(".divrow:eq(1)").hide();
                    break;
                case "message":
                    $("#reportType").html("发给我的私信");
                    $("#IsReport").attr("checked", "checked");
                    $("#reportcontent").show();
                    break;
                default:
            }

            $("#IsReport").click(function () {
                var ss = $(this).is(':checked');
                if (ss) {
                    $(".divrow:eq(1),#reportcontent").slideDown();
                } else {
                    $(".divrow:eq(1),#reportcontent").slideUp();
                }
            });
            $('#report_box').modal('show');

        });
    });
    // 取消
    $("#reportcancel").click(function () {
        $('#report_box').modal('hide');
    });
    //提交举报拉黑
    $("#reportbt").click(function () {
        var isreport = $("#IsReport").is(':checked');
        var des = $.trim($("#reportdes").val());
        var name = $("#reportedname").html();
        var id = $("#reportedname").attr("data-id");
        var relateId = $("#reporttype").attr("data-relateId");
        var type = $("#reporttype").val();
        var messagetype = "";

        //其他举报 需要提供举报信息的类型
        $("#reul input[type='radio']").each(function (n) {
            if ($(this).is(':checked')) {
                messagetype = $(this).next().html();
            }
        });
        if (messagetype == "" && isreport) {
            reportAlert("请选择举报类型！");
            return;
        }
        $(this).attr("disabled", "disabled");
        $.post("/Admin/ReportDeal", { "reportedId": id, "reportedName": name, "description": des, "isReport": isreport, "relateId": relateId, "reportType": type, "messageType": messagetype },
            function (data) {
                if (data == 1) {
                    reportAlert("拉黑/举报成功！");
                    $(".reportmessage").css("color", "blue");
                    var over = setTimeout(function () {
                        $('#report_box').modal('hide');
                        $("#reportbt").removeAttr("disabled");
                    }, 1000);
                }
                if (data == 2) {
                    reportAlert("此条信息已经举报过,谢谢合作！");
                    var over1 = setTimeout(function () {
                        $('#report_box').modal('hide');
                        $("#reportbt").removeAttr("disabled");
                    }, 1000);
                }
            });

    });

    function reportAlert(text) {
        $(".reportmessage").html(text).show();
        var st = setTimeout(function () {
            $(".reportmessage").fadeOut();
            $(".reportmessage").css("color", "red");
        }, 2000);
    }

    //打招呼处理-----------------------------------hi---------------------
    $(".hi").each(function (n) {
        $(this).click(function () {
            var userid = $(this).attr("data-userid");
            $.post("/Base/GetPersent", {}, function (data) {
                if (data < 0.3) {
                    alert("你的资料完整度小于30%,暂时不能使用打招呼功能,请先在个人主页里完善你的资料");
                    return;
                } else {

                    $.post("/Message/MakeaHello", { userid: userid }, function (datas) {//1成功 0表示已经打过了 2表示资料不够完整。不能打招呼。
                        if (datas == 1) { //说明打招呼成功！
                            //执行动画
                            $(".hi").eq(n).animate({
                                top: "-30px",
                                opacity: 0,
                            }, 1000, function () {
                                $(".hi").eq(n).css("opacity", 1).css("top", "0").show();
                            });
                            //禁止这个元素。
                        }
                        // 告之已经打过招呼。 
                        $(".hi").eq(n).css("color", "#4169e1").css("background-color", "#fff0f5").attr("disabled", "disabled").attr("title", "已打招呼");

                    });
                }
            });
        });
    });
    //赞的处理-------------------------------------praise-----------------------
    $(".parise").each(function (n) {
        $(this).click(function () {
            var userid = $(this).attr("data-userid");
            var stateid = $(this).attr("data-stateid");
            var type = parseInt($(this).attr("data-type"));

            $.post("/Base/GetPersent", {}, function (data) {
                if (data < 0.3) {
                    alert("你的资料完整度小于30%,暂时不能使用赞功能,请先在个人主页里完善你的资料");
                    return;
                } else {
                    $.post("/Message/MakeaParise", { "userid": userid, "stateid": stateid, statetype: type }, function (datas) {//1成功 0表示已经打过了 2表示资料不够完整。不能打招呼。
                        if (datas == 1) { //说明赞成功！
                            var samp = $(".parise").eq(n).find("samp");
                            samp.slideUp(500, function () {
                                var count = parseInt(samp.html()) + 1;
                                samp.html(count);
                                samp.slideDown();
                            });
                        }
                        $(".parise i").eq(n).css("background-color", "#ff00ff");
                        $(".parise").eq(n).attr("title", "你已经赞过了");
                    });
                }
            });
        });
    });
    //喜欢的处理-------------------------------------like----------------------
    $(".like").click(function () {
        var id = $(this).attr("data-userid");
        var likespan = $(this);
        var icon = "<i class='icon-heart icon-white'></i>";
        var info = $(this).siblings(".likeinfo");
        $.post("/Base/GetPersent", {}, function (iss) {
            if (iss < 0.3) {
                alert("你的资料完整度小于30%,暂时不能使用打招呼功能,请先在个人主页里完善你的资料");
                return;
            } else {
                $.post("/State/AddMyLover", { loverid: id }, function (data) {
                    if (data == 1) {
                        //说明喜欢成功，这个时候显示取消
                        likespan.html("已喜欢").css("background-color", "white").css("color", "#00ff7f").css("border", "1px gainsboro solid");
                        likespan.attr("title", "取消喜欢");
                        info.show().html("已喜欢她");//性别问题。
                        info.animate({ top: "-30px" }, 1000, function () {
                            var set = setTimeout(function () {
                                info.animate({ top: "0", }, 1000, function () {
                                    info.fadeOut();
                                });
                            }, 500);
                        });
                    } else {
                        //说明取消喜欢，这个时候显示喜欢
                        likespan.html(icon + "喜欢").css("background-color", "#ff69b4").css("color", "black").css("border", "1px white solid");
                        likespan.attr("title", "");
                        info.show().html("已不再喜欢她");//性别问题。
                        info.animate({ top: "-30px", }, 1000, function () {
                            var set = setTimeout(function () {
                                info.animate({ top: "0" }, 1000, function () {
                                    info.fadeOut();
                                });
                            }, 500);
                        });
                    }
                });
            }
        });
    });
    //不喜欢的处理-------------------------------------dislike---------------------
    $(".dislike").click(function () {
        var id = $(this).attr("data-userid");
        var parent = $(this).parent().parent().parent();
        $.post("/Base/GetPersent", {}, function (iss) {

            if (iss < 0.3) {
                alert("你的资料完整度小于30%,暂时不能使用打招呼功能,请先在个人主页里完善你的资料");
                return;
            } else {
                $.post("/State/AddDisLike", { userid: id }, function (data) {
                    if (data != 1) {
                        alert("解除成功！");
                    }
                    parent.fadeOut(1000, function () {
                        parent.remove();
                    });
                });
            }
        });
    });
    //图片上传-------------------------------------imgupload---------------------

    var imgbox = {};
    var childs = $(".imgcontainer:eq(0)").html();
    $(".uploadbox").click(function () {
        var tag = $(this).attr("data-count");
        if (tag == "1") return false;
        $("#imgFlie").click();
        imgbox = $(this);
        var stm1 = setInterval(function () {
            var imgstr = $("#imgFlie").val();
            if (imgstr != "") {
                clearInterval(stm1);
                $("#ImgForm input[type='submit']").click();
            }
        }, 500);
        return false;
    });

    // 取消照片
    $(".closespan").click(function (e) {
        e.stopPropagation();
        var sum = $(".uploadbox img").length;
        // 保证有一个框可以让用户再次选择 需要显示childs 设置data-count属性 必要的再隐藏
        var imgsrc = $(this).next().find("img").attr("src");
        $(this).next().html("").append(childs);
        // 隐藏关闭和成功提示
        $(this).hide();
        $(this).siblings(".infospan").hide();
        $(this).parent().attr("data-count", 0).insertBefore($(".inputdiv"));;

        if (sum < 4) {//说明此时是有一个再准备状态，这个可以直接做删除处理 
            // 设置data-count 并隐藏父级
            $(this).parent().hide();
            //需要移位 保证准备状态的总是在最后一个  
        }
        // 清除session 减少一个字符串
        if (imgsrc != undefined) {
            $.post("/User/DeleteImg", { str: imgsrc }, function () {

            });
        }
        if (sum == 1) {
            $('#Remark,#imgsubmit').attr("disabled", "disabled");
        }

    });

    //----------上传图片---------------------------------------------
    $("#imgsubmit").click(function () {
        var imgcontent = $.trim($("#Remark").val());
        $.post("/User/SaveImgs", { content: imgcontent }, function (data) {
            if (data == 1) {
                //清除content
                $(".imgcontainer").html(childs);
                //隐藏进度条和关闭键
                $(".closespan,.infospan").hide();
                $(".imguploadmessage").html("上传成功!");
                //禁止输入框和提交按钮
                $('#Remark,#imgsubmit').attr("disabled", "disabled");
                //去掉data-count 属性 显示第一个；
                $(".uploadbox").attr("data-count", 0).hide().eq(0).show();
                //清空
                $("#Remark").val("");
                var stt = setTimeout(function () {
                    $("#imgupload").modal('hide');
                    $(".imguploadmessage").html("");
                    clearTimeout(stt);
                }, 1000);
            }
        });
    });

    $('#ImgForm').ajaxForm({
        beforeSend: function () {
            imgbox.find(".infospan").show();
        },
        success: function (data) {
            $("#imgFlie").val("");//
            imgbox.find(".imgcontainer").html(data);//.hide()
            var img = imgbox.find(".imgcontainer").find("img").attr("src");
            if (img != undefined) {
                //解禁
                $('#Remark').removeAttr("disabled");
                $('#imgsubmit').removeAttr("disabled");
                imgbox.find(".infospan,.closespan").show();
                imgbox.find(".infospan").html("上传成功！");
                //显示下一个
                //imgbox.next().show();
                $(".uploadbox:hidden").eq(0).show();

                imgbox.attr("data-count", "1");
            } else {
                imgbox.find(".infospan,.closespan").hide();
            }
            // alert(img);
        }, complete: function (xhr) {
            $("#imgFlie").val("");
        }
    });

    //----------图片播放--------------------------------imgtigger-------------
    $(".imgtigger img").live("click", function () {
          var userid= $(this).parent().parent().attr("data-userid");
        var url = $(this).attr("src");
        var $ol = $("#myCarousel .carousel-indicators");
        var $div = $("#myCarousel .carousel-inner");
        $ol.empty();
        $div.empty();
        $.post("/State/GetImgInfos", { userid: userid, firstimgurl: url }, function(data) {
            var count = data.length;
            for (var i = 0; i < count; i++) {
                var li = "<li data-target='#myCarousel' data-slide-to=" + i + "></li>";
                var imgdiv = "<div class='item'><img src=" + data[i].ImgUrl + " /><div class='carousel-caption'><p>"
                   + data[i].Remark + "</div></div>";
                if (i == 0) {
                    li = "<li data-target='#myCarousel' class='active' data-slide-to=" + i + "></li>";
                    imgdiv = "<div class='item active'><img src=" + data[i].ImgUrl + " /><div class='carousel-caption'><p>"
                   + data[i].Remark + "</div></div>";
                };
                $ol.append(li);
                $div.append(imgdiv);
            }
            $("#CarouselBox").modal('show');
        });
    });
    
    $(".carclose").click(function() {
        $("#CarouselBox").modal('hide');
    });
    

});