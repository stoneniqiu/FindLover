$(function () {
    
    //显示和隐藏button
    $(".line").each(function (n) {
        var type = $("#indexType").html();// 检索的类型
        $("#navlist>li").eq(parseInt(type)).find("a").css("background-color", "#00bfff");
        $("#navlist>li").eq(parseInt(type)).find("a").css("color", "white");
      
        $(this).hover(function () {
            $(this).find(".rowdetail").find("div").show();
            $(this).find(".result").show();
            if (type == "2" || type == "3" || type == "4") {//仅看资料
                $(this).find(".btn-info").eq(0).hide();
                $(this).find(".btn-warning").eq(0).hide();
            }
            if (type == "1" || type == "5" || type == "6") {//仅看头像
                $(this).find(".btn-info").eq(1).hide();
                $(this).find(".btn-warning").eq(1).hide();
            }

            // 处理已审核过的。 比如头像已经审核过了，就不出现按钮了。 还需要重置按钮。
            var imgcheck = $(this).find(".imgbox").attr("data-check");//是否已经通过
            if (imgcheck == "True") {
                $(this).find(".btn-info").eq(0).hide();
                $(this).find(".btn-warning").eq(0).hide();
                if (type == "7") {
                    $(".btline").eq(n).find("a").eq(0).show();
                }
            }
            var check = $(this).find(".rowdetail").attr("data-check");//是否已经通过
            if (check == "True") {
                $(this).find(".btn-info").eq(1).hide();
                $(this).find(".btn-warning").eq(1).hide();
                if (type == "7") {
                    $(".btline").eq(n).find("a").eq(1).show();
                }
            }

        });

        $(this).mouseleave(function () {
            $(this).find(".result").hide();
            $(this).find(".rowdetail").find("div").hide();
        });
        var imgurl = $(this).find(".imgbox>img").attr("src");

        $(this).find(".rowdetail").find(".btn").each(function (i) {
            $(this).click(function () {
                var id = $(this).parent().attr("data-id");
                switch (i) {
                    case 0:
                        //图片合格
                        if (imgurl == "../../Content/Photos/luren.jpg") {
                            $(".img_result").eq(n).html("默认头像,操作无效！").css("color", "red");
                        } else {
                            $.post("/Admin/ValidImgOrInfo", { "userid": id, "result": true, "type": 0 }, function (data) {
                                if (data) {
                                    $(".img_result").eq(n).html("头像审核完成").css("color", "blue");
                                    $(".imgbox").eq(n).attr("data-valid", true);
                                    $(".imgbox").eq(n).attr("data-check", true);
                                    getImgCount();
                                }
                            });
                        
                        }
                        break;
                    case 1:
                        //图片不合格
                        if (imgurl == "../../Content/Photos/luren.jpg") {
                            $(".img_result").eq(n).html("默认头像,操作无效！").css("color", "red");
                        } else {
                            $.post("/Admin/ValidImgOrInfo", { "userid": id, "result": false, "type": 0 }, function (data) {
                                if (data) {
                                    $(".img_result").eq(n).html("头像审核完成").css("color", "blue");
                                    $(".imgbox").eq(n).attr("data-valid", false);
                                    $(".imgbox").eq(n).attr("data-check", true);
                                }
                            });
                        }
                        break;
                    case 2:
                        //资料合格
                        $.post("/Admin/ValidImgOrInfo", { "userid": id, "result": true, "type": 1 }, function (data) {
                            if (data) {
                                $(".info_result").eq(n).html("资料审核完成").css("color", "blue");
                                $(".rowdetail").eq(n).attr("data-check", true);
                                $(".rowdetail").eq(n).attr("data-valid", true);
                                getInfoCount();
                            }
                        });
                        break;
                    case 3:
                        //资料不合格
                        $.post("/Admin/ValidImgOrInfo", { "userid": id, "result": false, "type": 1 }, function (data) {
                            if (data) {
                                $(".info_result").eq(n).html("资料审核完成").css("color", "blue");
                                $(".rowdetail").eq(n).attr("data-check", true);
                                $(".rowdetail").eq(n).attr("data-valid", false);
                            }
                        });
                        break;
                    default:
                }
                slidupLine(n);
            });

        });
       // 资料统计 
    });

    function  slidupLine(n) {
        if ($(".imgbox").eq(n).attr("data-check") == "True" && $(".rowdetail").eq(n).attr("data-check") == "True") {
            var st = setTimeout(function () {
                $(".line").eq(n).slideUp();
            }, 1500);
        }
    }

    var usernum = $("#usercout").html();
    if (usernum == "0") {
        $(".nowork").show();
    }
    // 返回管理员审核的图片数
    function getImgCount() {
        $.post("/Admin/GetImgCount", function(data) {
            $("#imgcout").html(data);
        });
    }
    // 返回管理员审核的资料数
  
    function getInfoCount() {
        $.post("/Admin/GetInfoCount", function (data) {
            $("#infocount").html(data);
        });
    }
  
    //审核的结果和成绩

})