$(function () {
    //style="margin-left: 10px;"
    $(".imgbox").each(function (n) {
        if (n % 4 == 0) {
            $(this).css("margin-left", "10px");
        }
    });

    $(".spanbox>span").each(function (n) {

        // 下拉框的弹出和隐藏。
        $(this).click(function () {//   $(this).css("border-bottom", "1px white solid");

            var flag = $(this).parent().parent().hasClass("conditon");
            if ($(".spanhide").eq(n).is(":visible")) {
                if (flag) {
                    $(this).css("border-bottom", "1px gainsboro solid");
                }
                togglechevronDonw($(this));
                $(".spanbox").eq(n).css("z-index", "0");

            } else {
                if (flag) {
                    $(this).css("border-bottom", "1px white solid"); // 这句要区别
                }
                togglechevronUp($(this));
                $(".spanbox").eq(n).css("z-index", "23");

                $(".spanhide").each(function (i1) {
                    if (i1 != n) {
                        var flag1 = $(this).parent().parent().hasClass("conditon");
                        $(this).hide();
                        if (flag1) {
                            $(".spanbox>span").eq(i1).css("border-bottom", "1px gainsboro solid");// 这句要区别
                        }
                        togglechevronDonw($(".spanbox>span>.icon-chevron-down").eq(i));
                        $(".spanbox").eq(i1).css("z-index", "0");
                    }
                });

            }
            $(".spanhide").eq(n).toggle();
        });
    });
    // 未选条件搜索
    $(".conditon .spanbox a").each(function (n) {
        $(this).click(function () {
            var x = n + 2;
            var text = "";
            switch (n) {
                case 0:
                    var edu = $(".edusl:eq(1)").val();
                    text = $("#Educationcheck").is(":checked") ? (edu != "大专以下" ? edu + "及以上" : "学历不限") : edu;
                    break;
                case 1:
                    text = $(".housl:eq(1)").val();
                    break;
                case 2:
                    text = $(".incomesl:eq(1)").val();
                    break;
                case 3:
                    text = $(".carsl:eq(1)").val();
                    break;
                case 4:
                    var heilow = $(".heightsl:eq(2)").val();
                    var heiup = $(".heightsl:eq(3)").val();
                    text = heiup != "不限" ? heilow.split('厘')[0] + "到" + heiup : heilow + "以上";
                    break;
                case 5:
                    text = $(".consl:eq(1)").val();
                    break;
                case 6:
                    var prov = $(".prosl:eq(1)").val();
                    var city = $(".citiysl:eq(1)").val();
                    text = city != "不限" ? "(籍贯)" + prov + city : "(籍贯)" + prov;
                    $(".selectedconditon .spanbox>span>span").eq(8).attr("title", text);
                    //lenth 最大为10 否则就溢出了 如果要大于10 要处理
                    if (text.length > 10) {
                        text = text.substring(9, length) + "...";
                    }
                    break;
                case 7:
                    text = $(".peoplesl:eq(1)").val();
                    break;
                case 8:
                    text = $(".stasl:eq(1)").val();
                    break;
                default:
            }

            $(".selectedconditon .spanbox>span>span").eq(x).html(text);

            togglechevronDonw($(".conditon .spanbox>span").eq(n));
            $(".conditon .spanbox").eq(n).hide();
            $(".conditon .spanbox>span").eq(n).css("border-bottom", "1px gainsboro solid");// 这句要区别
            $(".conditon .spanbox .spanhide").eq(n).hide();
            $(".conditon .spanbox").eq(n).css("z-index", "0");
            $(".selectedconditon .spanbox").eq(x).show();
            search();
        });
    });
    // 已选条件搜索 点击确定
    $(".selectedconditon .spanbox a").each(function (n) {
        $(this).click(function () {
          
            var text="";
            switch (n) {
            case 0:
                text = "(住在)" + $(".selectedconditon .linewarp").eq(n).find("select").val();
                break;
                case 1:
                    var agelow = $(".agesl:eq(0)").val();
                    var ageup = $(".agesl:eq(1)").val();
                    text = ageup != "不限" ? agelow.split('岁')[0] + "到" + ageup : agelow + "以上";
                    break;
                case 2:
                    var edu = $(".edusl:eq(0)").val();
                    text = $("#Educationcheck1").is(":checked") ? (edu != "大专以下" ? edu + "及以上" : "学历不限") : edu;
                    break;
                case 3:
                    text = $(".housl:eq(0)").val();
                    break;
                case 4:
                    text = $(".incomesl:eq(0)").val();
                    break;
                case 5:
                    text = $(".carsl:eq(0)").val();
                    break;
                case 6:
                    var heilow = $(".heightsl:eq(0)").val();
                    var heiup = $(".heightsl:eq(1)").val();
                    text = heiup != "不限" ? heilow.split('厘')[0] + "到" + heiup : heilow + "以上";
                    break;
                case 7:
                    text = $(".consl:eq(0)").val();
                    break;
                case 8:
                    var prov = $(".prosl:eq(0)").val();
                    var city = $(".citiysl:eq(0)").val();
                    text = city != "不限" ? "(籍贯)" + prov + city : "(籍贯)" + prov;
                    $(".selectedconditon .spanbox>span>span").eq(8).attr("title", text);
                    //lenth 最大为10 否则就溢出了 如果要大于10 要处理
                    if (text.length > 10) {
                        text = text.substring(9, length) + "...";
                    }
                    break;
                case 9:
                    text = $(".peoplesl:eq(0)").val();
                    break;
                    case 10:
                        text = $(".stasl:eq(0)").val();
                        break;
            default:
            }

            $(".selectedconditon .spanbox>span>span").eq(n).html(text);

            search();
            togglechevronDonw($(".selectedconditon .spanbox>span").eq(n));
            $(".selectedconditon .spanbox .spanhide").eq(n).hide();
            $(".selectedconditon .spanbox").eq(n).css("z-index", "0");

        });
    });
    function search() {
        var $span = $(".selectedconditon .spanbox span>span");
        //居住地
        var residenceCity = $span.eq(0).html().split(')')[1];
        $("#ResidenceCity").val(residenceCity);
        //年龄,有xx岁到xx岁 有 xx岁以上
        var age = $span.eq(1).html();
        if (age.indexOf('到')>0) {
            $("#AgeUp").val(age.split('到')[0]);
            $("#AgeLow").val(age.split('到')[1].split('岁')[0]);
        } else {
            $("#AgeUp").val(age.split('岁')[0]);
            $("#AgeLow").val('0');
        }
        //处理学历 
        $("#Education").val($span.eq(2).html());
        //住房
        $("#Housing").val($span.eq(3).html());
        //月收入
        $("#MonthlyIncome").val($span.eq(4).html());
        //购车
        $("#Car").val($span.eq(5).html());
        //身高
        var hei = $span.eq(6).html();
      
        if (hei.indexOf('到') > 0) {
            $("#HightUp").val(hei.split('到')[0]);
            $("#HightLow").val(hei.split('到')[1].split('厘')[0]);
        } else {
            $("#HightUp").val(hei.split('厘')[0]);
            $("#HightLow").val('0');
        }
        //星座
        $("#Constellation").val($span.eq(7).html());
        //籍贯
        var place = $span.eq(8).html();
        var ptitle = $span.eq(8).attr("title");
        if (place.indexOf('...') > 0) {
            $("#NativePlace").val(ptitle.split(")")[1]);
        } else {
            $("#NativePlace").val(place.split(")")[1]);
        }
        //民族
        $("#People").val($span.eq(9).html());
        //状态
        $("#State").val($span.eq(10).html());
      
        //条件都处理之后，触发提交---------------------------
        $("#goSearch").click();
    }



    //关闭已有条件 触发搜索。 
    $(".selectedconditon .spanbox span samp").each(function (n) {
        $(this).click(function (e) {
            $(".selectedconditon .spanbox").eq(n + 2).hide();
            e.stopPropagation();//不显示下拉框直接关闭这个条件 并显示在下面的更多条件中
            $(".conditon .spanbox").eq(n).show();
            $(".selectedconditon .spanbox>span>span").eq(n+2).html("");
            
            search();
        });

    });

    var pros = ['北京', '上海', '天津', '重庆', '黑龙江', '吉林', '山东', '山西', '陕西', '河北', '河南', '湖北', '湖南', '海南', '江苏', '江西', '广东', '广西', '云南', '贵州',
       '四川', '内蒙古', '宁夏', '甘肃', '青海', '安徽', '浙江', '福建', '西藏', '新疆', '台湾', '香港', '澳门'];
    var stas = ['未婚,寻觅中', '离异,寻觅中', '丧偶,寻觅中'];
    var edus = ['大专以下', '大专', '本科', '硕士', '博士'];
    var hos = ['已购房', '租房', '公司宿舍', '和家人同住'];
    var cars = ['已购车', '未购车'];
    var incomes = ['3000元以下', '3000元以上', '4000元以上', '5000元以上', '6000元以上', '8000元以上', "10000元以上", "20000元以上"];
    var cons = ['摩羯座', '水平座', '双鱼座', '白羊座', '金牛座', '双子座', '巨蟹座', '狮子座', '处女座', '天秤座', '天蝎座', '射手座'];

    //身高问题处理
    $(".heightsl:odd").append("<option>不限</option>");
    for (var i = 150; i < 210; i++) {
        $(".heightsl").append("<option>" + i + "厘米</option>");
    }
    $(".heightsl:eq(0)").change(function() {
        var hei = parseInt($(this).val().split('厘')) + 1;
        var heiup = $(".heightsl:eq(1)").val();
        $(".heightsl:eq(1) option:gt(0)").remove();
        for (var a1 = hei; a1 <= 210; a1++) {
            $(".heightsl:eq(1)").append("<option>" + a1 + "厘米</option>");
        }
        $(".heightsl:eq(1)").val(heiup);//有这个值 就选择。
    });

    $(".heightsl:eq(2)").change(function () {
        var hei = parseInt($(this).val().split('厘')) + 1;
        var heiup = $(".heightsl:eq(3)").val();
        $(".heightsl:eq(3) option:gt(2)").remove();
        for (var a1 = hei; a1 <= 210; a1++) {
            $(".heightsl:eq(3)").append("<option>" + a1 + "厘米</option>");
        }
        $(".heightsl:eq(3)").val(heiup);//有这个值 就选择。
    });


    // 处理年龄的选择问题
    $(".agesl:odd").append("<option>不限</option>");
    for (var j = 18; j <= 60; j++) {
        $(".agesl").append("<option>" + j + "岁</option>");
    }
    $(".agesl:eq(0)").change(function() {
        var agelow = parseInt($(this).val().split('岁')) + 1;
        var ageup =  $(".agesl:eq(1)").val();
        
        $(".agesl:eq(1) option:gt(0)").remove();
        for (var a = agelow; a <= 60; a++) {
            $(".agesl:eq(1)").append("<option>" + a + "岁</option>");
        }
        $(".agesl:eq(1)").val(ageup);//有这个值 就选择。
    });


     
    var beijing = ['不限', '东城区', '西城区', '崇文区', '宣武区', '朝阳区', '丰台区', '石景山区', '海淀区', '门头沟区', '房山区', '通州区', '顺义区', '昌平区', '大兴区',
       '平谷区', '怀柔区', '密云县', '延庆县'];
    var shanghai = ['不限', '黄浦区', '卢湾区', '徐汇区', '长宁区', '静安区', '普陀区', '闸北区', '虹口区', '杨浦区', '闵行区', '宝山区', '嘉定区', '浦东新区', '金山区',
        '松江区', '青浦区', '南汇区', '奉贤区', '崇明县'];
    var tianjin = ['不限', '和平区', '河东区', '河西区', '南开区', '河北区', '红桥区', '塘沽区', '汉沽区', '大港区', '东丽区', '西青区', '津南区', '北辰区', '武清区', '宝坻区', '宁河县', '静海县', '蓟县'];
    var chongqing = ('不限 万州区 涪陵区 渝中区 大渡口区 江北区 沙坪坝区 九龙坡区 南岸区 北碚区 万盛区 双桥区 渝北区 巴南区 黔江区 长寿区 江津区 合川区 永川区 南川区 綦江县 潼南县 铜梁县 大足县 荣昌县 璧山县 梁平县 城口县 丰都县 垫江县 ' +
        '武隆县 忠县 开县 云阳县 奉节县 巫山县 巫溪县 石柱土家族自治县 秀山土 家族苗族自治县 酉阳土家族苗族自治县 彭水苗族土家族自治县').split(' ');
    var heilongjiang = ('不限 哈尔滨市 齐齐哈尔市 鸡西市 鹤岗市 双鸭山市 大庆市 伊春市 佳木斯市 七台河市 牡丹江市 黑河市 绥化市 大兴安岭地区').split(' ');
    var jilin = ('不限 长春市 吉林市 四平市 辽源市 通化市 白山市 松原市 白城市 延边朝鲜族自治州 辽宁 沈阳市 大连市 鞍山市 抚顺市 本溪市 丹东市 锦州市 营口市 阜新市 辽阳市 盘锦市 铁岭市 朝阳市 葫芦岛市').split(' ');
    var shandong = '不限 济南市 青岛市 淄博市 枣庄市 东营市 烟台市 潍坊市 济宁市 泰安市 威海市 日照市 莱芜市 临沂市 德州市 聊城市 滨州市 菏泽市'.split(' ');
    var shanxi = '不限 太原市 大同市 阳泉市 长治市 晋城市 朔州市 晋中市 运城市 忻州市 临汾市 吕梁市'.split(' ');
    var shanxiX = '不限 西安市 铜川市 宝鸡市 咸阳市 渭南市 延安市 汉中市 榆林市 安康市 商洛市'.split(' ');
    var hebei = '不限 石家庄市 唐山市 秦皇岛市 邯郸市 邢台市 保定市 张家口市 承德市 沧州市 廊坊市 衡水市'.split(' ');
    var henan = '不限 郑州市 开封市 洛阳市 平顶山市 安阳市 鹤壁市 新乡市 焦作市 济源市 濮阳市 许昌市 漯河市 三门峡市 南阳市 商丘市 信阳市 周口市 驻马店市'.split(' ');
    var hubei = '不限 武汉市 黄石市 十堰市 宜昌市 襄樊市 鄂州市 荆门市 孝感市 荆州市 黄冈市 咸宁市 随州市 恩施土家族苗族自治州 仙桃市 潜江市 天门市 神农架林区'.split(' ');
    var hunan = '不限 长沙市 株洲市 湘潭市 衡阳市 邵阳市 岳阳市 常德市 张家界市 益阳市 郴州市 永州市 怀化市 娄底市 湘西土家族苗族自治州'.split(' ');
    var hainan = '不限 海口市 三亚市 五指山市 琼海市 儋州市 文昌市 万宁市 东方市 定安县 屯昌县 澄迈县 临高县 白沙黎族自治县 昌江黎族自治县 乐东黎族自治县 陵水黎族自治县 保亭黎族苗族自治县 琼中黎族苗族自治县'.split(' ');
    var jiangsu = '不限 南京市 无锡市 徐州市 常州市 苏州市 南通市 连云港市 淮安市 盐城市 扬州市 镇江市 泰州市 宿迁市'.split(' ');
    var jiangxi = '不限 南昌市 景德镇市 萍乡市 九江市 新余市 鹰潭市 赣州市 吉安市 宜春市 抚州市 上饶市'.split(' ');
    var guangdong = '不限 广州市 韶关市 深圳市 珠海市 汕头市 佛山市 江门市 湛江市 茂名市 肇庆市 惠州市 梅州市 汕尾市 河源市 阳江市 清远市 东莞市 中山市 潮州市 揭阳市 云浮市'.split(' ');
    var guangxi = '不限 南宁市 柳州市 桂林市 梧州市 北海市 防城港市 钦州市 贵港市 玉林市 百色市 贺州市 河池市 来宾市 崇左市'.split(' ');
    var yunnan = '不限 昆明市 曲靖市 玉溪市 保山市 昭通市 丽江市 思茅市 临沧市 楚雄彝族自治州 红河哈尼族彝族自治州 文山壮族苗族自治州 西双版纳傣族自治州 大理白族自治州 德宏傣族景颇族自治州 怒江傈僳族自治州 迪庆藏族自治州'.split(' ');
    var guizhou = '不限 贵阳市 六盘水市 遵义市 安顺市 铜仁地区 黔西南布依族苗族自治州 毕节地区 黔东南苗族侗族自治州 黔南布依族苗族自治州'.split(' ');
    var sichuan = '不限 成都市 自贡市 攀枝花市 泸州市 德阳市 绵阳市 广元市 遂宁市 内江市 乐山市 南充市 眉山市 宜宾市 广安市 达州市 雅安市 巴中市 资阳市 阿坝藏族羌族自治州 甘孜藏族自治州 凉山彝族自治州'.split(' ');
    var neimenggu = '不限 呼和浩特市 包头市 乌海市 赤峰市 通辽市 鄂尔多斯市 呼伦贝尔市 巴彦淖尔市 乌兰察布市 兴安盟 锡林郭勒盟 阿拉善盟'.split(' ');
    var ningxia = '不限 银川市 石嘴山市 吴忠市 固原市 中卫市'.split(' ');
    var qignhai = '不限 西宁市 海东地区 海北藏族自治州 黄南藏族自治州 海南藏族自治州 果洛藏族自治州 玉树藏族自治州 海西蒙古族藏族自治州'.split(' ');
    var gansu = '不限 兰州市 嘉峪关市 金昌市 白银市 天水市 武威市 张掖市 平凉市 酒泉市 庆阳市 定西市 陇南市 临夏回族自治州 甘南藏族自治州'.split(' ');
    var anhui = '不限 合肥市 芜湖市 蚌埠市 淮南市 马鞍山市 淮北市 铜陵市 安庆市 黄山市 滁州市 阜阳市 宿州市 巢湖市 六安市 亳州市 池州市 宣城市'.split(' ');
    var zhejiang = '不限 杭州市 宁波市 温州市 嘉兴市 湖州市 绍兴市 金华市 衢州市 舟山市 台州市 丽水市'.split(' ');
    var fujian = '不限 福州市 厦门市 莆田市 三明市 泉州市 漳州市 南平市 龙岩市 宁德市'.split(' ');
    var xizang = '不限 拉萨市 昌都地区 山南地区新疆 乌鲁木齐市 克拉玛依市 吐鲁番地区 哈密地区日喀则地区 那曲地区 阿里地区 林芝地区 昌吉回族自治州 博尔塔拉蒙古自治州 巴音郭楞蒙古自治州 阿克苏地区 克孜勒苏柯尔克孜自治州 喀什地区 和田地区 伊犁哈萨克自治州 塔城地区 阿勒泰地区 石河子市 阿拉尔市 图木舒克市 五家渠市'.split(' ');
    var xinjiang = ['不限', '乌鲁木齐市', '克拉玛依市', '吐鲁番地区', '哈密地区', '昌吉回族自治州', '博尔塔拉蒙古自治州', '巴音郭楞蒙古自治州', '阿克苏地区', '克孜勒苏柯尔克孜自治州',
        '喀什地区', '和田地区', '伊犁哈萨克自治州', '塔城地区', '阿勒泰地区', '省直辖行政单位'];
    var taiwan = '不限 台北市 高雄市 基隆市 台中市 台南市 新竹市 嘉义市'.split(' ');
    var xianggang = '不限 中西区 湾仔区 东区 南区 油尖旺区 深水埗区 九龙城区 黄大仙区 观塘区 荃湾区 葵青区 沙田区 西贡区 大埔区 北区 元朗区 屯门区 离岛区'.split(' ');
    var aomen = ['澳门'];
    var peoples = ['汉族', '壮族', '满族', '回族', '苗族', '土家族', '维吾尔族', '彝族', '藏族', '布依族', '侗族', '瑶族', '朝鲜族', '白族', '傣族', '高山族',
  '畲族', '僳僳族', '仡佬族', '东乡族', '水族', '佤族', '羌族', '仫佬族'];


  
    initSelete($(".prosl"), pros);
    initSelete($(".edusl"), edus);
    initSelete($(".housl"), hos);
    initSelete($(".carsl"), cars);
    initSelete($(".incomesl"), incomes);
    initSelete($(".consl"), cons);
    initSelete($(".stasl"), stas);
    initSelete($(".peoplesl"), peoples);
 
    $(".prosl").change(function () {
        var pro = $(this).val();
       // $("#NativePlace").val(pro);
        switch (pro) {
            case "请选择":
                $(".citiysl").empty();
                initSelete($("#Provience"), pros);
                break;
            case "北京":
                initSelete($(".citiysl"), beijing);
                break;
            case "上海":
                initSelete($(".citiysl"), shanghai);
                break;
            case "天津":
                initSelete($(".citiysl"), tianjin);
                break;
            case "重庆":
                initSelete($(".citiysl"), chongqing);
                break;
            case "黑龙江":
                initSelete($(".citiysl"), heilongjiang);
                break;
            case "吉林":
                initSelete($(".citiysl"), jilin);
                break;
            case "山东":
                initSelete($(".citiysl"), shandong);
                break;
            case "山西":
                initSelete($(".citiysl"), shanxi);
                break;
            case "陕西":
                initSelete($(".citiysl"), shanxiX);
                break;
            case "河北":
                initSelete($(".citiysl"), hebei);
                break;
            case "河南":
                initSelete($(".citiysl"), henan);
                break;
            case "湖北":
                initSelete($(".citiysl"), hubei);
                break;
            case "湖南":
                initSelete($(".citiysl"), hunan);
                break;
            case "海南":
                initSelete($(".citiysl"), hainan);
                break;
            case "江苏":
                initSelete($(".citiysl"), jiangsu);
                break;
            case "江西":
                initSelete($(".citiysl"), jiangxi);
                break;
            case "广东":
                initSelete($(".citiysl"), guangdong);
                break;
            case "广西":
                initSelete($(".citiysl"), guangxi);
                break;
            case "云南":
                initSelete($(".citiysl"), yunnan);
                break;
            case "四川":
                initSelete($(".citiysl"), sichuan);
                break;
            case "内蒙古":
                initSelete($(".citiysl"), neimenggu);
                break;
            case "宁夏":
                initSelete($(".citiysl"), ningxia);
                break;
            case "贵州":
                initSelete($(".citiysl"), guizhou);
                break;
            case "甘肃":
                initSelete($(".citiysl"), gansu);
                break;
            case "青海":
                initSelete($(".citiysl"), qignhai);
                break;
            case "安徽":
                initSelete($(".citiysl"), anhui);
                break;
            case "浙江":
                initSelete($(".citiysl"), zhejiang);
                break;
            case "福建":
                initSelete($(".citiysl"), fujian);
                break;
            case "西藏":
                initSelete($(".citiysl"), xizang);
                break;
            case "新疆":
                initSelete($(".citiysl"), xinjiang);
                break;
            case "台湾":
                initSelete($(".citiysl"), taiwan);
                break;
            case "香港":
                initSelete($(".citiysl"), xianggang);
                break;
            case "澳门":
                initSelete($(".citiysl"), aomen);
                break;
            default:
        }

    });
    $(".prosl").change();
    //初始化下拉框
    function initSelete(sl, strs) {
        sl.empty();
        for (var a = 0; a < strs.length; a++) {
            sl.append("<option>" + strs[a] + "</option>");
        }
    }
    function togglechevronDonw(object) {
        object.find(".icon-chevron-down").show();
        object.find(".icon-chevron-up").hide();
    }
    function togglechevronUp(object) {
        object.find(".icon-chevron-down").hide();
        object.find(".icon-chevron-up").show();
    }
    // 产生搜索
    
    $('#Form1').ajaxForm({
        success: function (data) {
            $("#userpage").html(data);
        }, complete: function (xhr) {
           
        }
    });

});