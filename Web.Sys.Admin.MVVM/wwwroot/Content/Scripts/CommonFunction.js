
function ConvertToTreeData(data) {//转为tree所须的格式
    $.each(data, function (i, n) {
        n = ConvertToTreeDataSon(n);
    })
    return data;
}
function ConvertToTreeDataSon(obj) {
    if (obj.children != null) {
        obj.id = obj.Id;
        obj.text = obj.Name;
        var ss = obj.children;
        $.each(obj.children, function (j, d) {
            d = ConvertToTreeDataSon(d);
        })
    } else {
        obj.id = obj.Id;
        obj.text = obj.Name;
    }
    return obj;
}

//html字符串正则替换
function htmlEncode(str) {
    var _str = '';
    if (str.length == 0) return '';
    _str = str.replace(new RegExp('<', 'gm'), '&lt');
    _str = _str.replace(new RegExp('>', 'gm'), '&gt');

    return _str;
}
//html字符串正则替换
function htmlDecode(str) {
    var _str = '';
    if (str.length == 0) return '';
    _str = str.replace(new RegExp('&lt', 'gm'), '<');
    _str = _str.replace(new RegExp('&gt', 'gm'), '>');

    return _str;
}
//去掉所有的html标记
function delHtmlTag(str, len) {
    if (str != undefined) {
        var title = str.replace(/<[^>]+>/g, "");//去掉所有的html标记
        if (title.length > len) {
            title = title.substring(0, len) + "...";
        }
        return title;
    } else {
        return "";
    }

}

//字符串截取
function StringSub(str, len) {
    if (str != undefined) {
        if (str.length > len) {
            str = str.substring(0, len) + "...";
        }
        return str;
    } else {
        return "";
    }
}

function clearNoNum(obj) {
    //先把非数字的都替换掉，除了数字和. 
    obj.value = obj.value.replace(/[^\d.]/g, "");
    //必须保证第一个为数字而不是. 
    obj.value = obj.value.replace(/^\./g, "");
    //保证只有出现一个.而没有多个. 
    obj.value = obj.value.replace(/\.{2,}/g, ".");
    //保证.只出现一次，而不能出现两次以上
    obj.value = obj.value.replace(".", "$#$").replace(/\./g, "").replace("$#$", ".");
}

function clearNoNumberContainDot(obj) {
    //先把非数字的都替换掉
    obj.value = obj.value.replace(/[^\d]/g, "");
}
function paramString2obj(serializedParams) {

    var obj = {};
    function evalThem(str) {
        var attributeName = str.split("=")[0];
        var attributeValue = str.split("=")[1];
        if (!attributeValue) {
            return;
        }

        var array = attributeName.split(".");
        for (var i = 1; i < array.length; i++) {
            var tmpArray = Array();
            tmpArray.push("obj");
            for (var j = 0; j < i; j++) {
                tmpArray.push(array[j]);
            };
            var evalString = tmpArray.join(".");
            // alert(evalString);
            if (!eval(evalString)) {
                eval(evalString + "={};");
            }
        };

        eval("obj." + attributeName + "='" + attributeValue + "';");

    };

    var properties = serializedParams.split("&");
    for (var i = 0; i < properties.length; i++) {
        evalThem(properties[i]);
    };

    return obj;
}


$.fn.form2json = function () {
    var serializedParams = this.serialize();
    var obj = paramString2obj(serializedParams);
    return JSON.stringify(obj);
}

function alerts(str, type) {

    if (type == 1)
        $.messager.alert('系统提示', str);
    if (type == 2)
        $.messager.alert('系统提示', str, 'info');
    if (type == 3)
        $.messager.alert('系统提示', str, 'question');
    if (type == 4)
        $.messager.alert('系统提示', str, 'warning');
    if (type == 5)
        $.messager.alert('系统提示', str, 'error');
}
function alertUrl(str, url) {

    $.messager.alert('系统提示', str, 'warning');
    setTimeout("location.href = '" + url + "'", 1000)
}
function clearNoNum(obj) {
    //先把非数字的都替换掉，除了数字和. 
    obj.value = obj.value.replace(/[^\d.]/g, "");
    //必须保证第一个为数字而不是. 
    obj.value = obj.value.replace(/^\./g, "");
    //保证只有出现一个.而没有多个. 
    obj.value = obj.value.replace(/\.{2,}/g, ".");
    //保证.只出现一次，而不能出现两次以上
    obj.value = obj.value.replace(".", "$#$").replace(/\./g, "").replace("$#$", ".");
}
var DataBaseFunction = {
    ClearForm: function (formId) {
        $("#" + formId).form("clear");
        //$("#" + formId + " :hidden").val("");
        //$("#" + formId + " :text").val("");
        //$("#" + formId + " textarea").val("");

    },
    GetFormData: function (formId) {
        //获取表单数据
        var postData = $("#" + formId).serializeArray();
        var data = [];
        for (var i = 0; i < postData.length; i++) {
            var IsAdd = true;
            for (var j = 0; j < data.length; j++) {

                if (data[j].name == postData[i].name) {
                    data[j].value += "%&" + postData[i].value//重复键用"%&"隔开
                    IsAdd = false;
                    break;
                }
            }
            if (IsAdd) {
                data.push(postData[i]);
            }
        }
        return data;
    },
    BindForm: function (formId, data) {//绑定数据给表单
        for (items in data) {
            var dd = $("#" + formId + " [name='" + items + "']");
            if (dd != null) {
                var ty = dd.attr("type");
                if (ty != null && ty == "radio") {

                    controlSet.RadioBindSelect(formId, items, data[items]);
                    // dd.removeAttr("CHECKED");
                    //$("[name=" + items + "][value=" + data[items] + "]").attr("checked", true); ie9以上失效
                } else if (ty != null && ty == "checkbox") {
                    dd.removeAttr("CHECKED");
                    var strdata = data[items].split("%&");
                    for (var k = 0; k < strdata.length; k++) {
                        $("[name=" + items + "][value=" + strdata[k] + "]").prop("checked", true);

                    }

                } else if (dd.get(0) != undefined && dd.get(0).tagName == "SPAN") {
                    dd.html(data[items]);
                } else {
                    dd.val(data[items]);
                }
            }
        }
    },
    deleteInfo: function (datagridId, Idfield, Namefield) {
        //得到按钮选择的数据的ID
        var rows = $("#" + datagridId).datagrid("getSelections");
        //首先判断按钮是否已经选择了需要删除的数据,然后循环将按钮选择的数据传递到后台
        if (rows.length >= 1) {
            //遍历出按钮选择的数据的信息，这就是按钮按钮选择删除的按钮ID的信息
            var ids = "";   //1,2,3,4,5
            for (var i = 0; i < rows.length; i++) {
                //异步将删除的ID发送到后台，后台删除之后，返回前台，前台刷洗表格
                ids +="'"+rows[i][Idfield] + "',";
            }
            //最后去掉最后的那一个,
            ids = ids.substring(0, ids.length - 1);
            //获取按钮选择的按钮信息，如果按钮选择了已经登录的按钮的话需要给出不能删除的信息
            var Name = "";
            for (var i = 0; i < rows.length; i++) {
                Name += rows[i][Namefield] + ",";
            }
            Name = Name.substring(0, Name.length - 1);
            //首先取出来到底是直接删除还是伪删除发送异步请求的地址和是否是伪删除的参数
            var postData = "";

            postData = {
                IDSet: ids
            };
            //}
            //else {
            //    postData = {
            //        ID: ids, Not: not
            //    }
            //}
            //然后确认发送异步请求的信息到后台删除数据
            $.messager.confirm("删除信息", "您确认删除<font color='red' size='3'>" + Name + "</font>吗？", function (Delete) {
                if (Delete) {
                    $.get(BaseUrl + "Del", postData, function (data) {
                        alerts(data.msg, 2);
                        if (data.code > 0) {
                            $("#" + datagridId).datagrid("reload");
                            $("#" + datagridId).datagrid("clearSelections");
                        }

                    });
                }
            });
        }
        else {
            alert("请选择你要删除的数据");
        }

    },
    Search: function (FormId, DataGridId) {
        var FData = $("#" + FormId).serializeArray();
        var data = [];
        for (var i = 0; i < FData.length; i++) {
            if (FData[i].value != "") {
                data.push(FData[i]);
            }
        }
        var result = "";
        for (var i = 0; i < data.length; i++) {
            result += data[i].name + ":" + data[i].value + "█";
        }
        result = result.substring(0, result.length - 1);
        $('#' + DataGridId).datagrid('load', {
            sqlSet: result
        });
    },
    Search2: function (FormId, DataGridId) {
        var FData = $("#" + FormId).serializeArray();
        var FData2 = $("#" + FormId).form2json();
        var data = [];
        var result = "";
        for (var i = 0; i < FData.length; i++) {
            if (FData[i].value != "") {
                //  data.push(FData[i]);
                result += "{field: '" + FData[i].name + "',value:'" + FData[i].value + "',op:'equal'},";

            }
        }


        //for (var i = 0; i < data.length; i++) {
        //    result += data[i].name + ":" + data[i].value + "█";
        //}
        result = result.substring(0, result.length - 1);
        result = "{op: 'and',rules:[" + result + "]}";
        //  var obj = eval("(" + result + ")");
        $('#' + DataGridId).datagrid('load', {
            sqlSet: result
        });
    }

}


var DataGridCheck = {//DataGrid选择操作 保存数据 
    Idfield: 'Id',//Id列名
    dataGridSelecttor: '#dg',//datagrid选择器
    checkedItems: [],//选择的数据
    addcheckItem: function () {//添加项
        var row = $(DataGridCheck.dataGridSelecttor).datagrid('getChecked');
        for (var i = 0; i < row.length; i++) {
            if (DataGridCheck.findCheckedItem(row[i][DataGridCheck.Idfield]) == -1) {
                DataGridCheck.checkedItems.push(row[i]);
            }
        }
    }, removeSingleItem: function (rowIndex, rowData) {//移除单项
        var k = DataGridCheck.findCheckedItem(rowData[DataGridCheck.Idfield]);
        if (k != -1) {
            DataGridCheck.checkedItems.splice(k, 1);
        }
    },
    removeAllItem: function (row) {//移除所有
        for (var i = 0; i < row.length; i++) {
            var k = DataGridCheck.findCheckedItem(row[i][DataGridCheck.Idfield]);
            if (k != -1) {
                DataGridCheck.checkedItems.splice(k, 1);
            }
        }
    },
    SetcheckItem: function (dataGridSelecttor, Idfield) {//设置选中
        for (var i = 0; i < DataGridCheck.checkedItems.length; i++) {
            $(DataGridCheck.dataGridSelecttor).datagrid('selectRecord', DataGridCheck.checkedItems[i][DataGridCheck.Idfield]); //根据id选中行 
        }
    },
    findCheckedItem: function (IdValue) {//是否存在
        for (var i = 0; i < DataGridCheck.checkedItems.length; i++) {
            if (DataGridCheck.checkedItems[i][DataGridCheck.Idfield] == IdValue) return i;
        }
        return -1;
    }

};
//html 控件操作
var controlSet = {
    RadioSetSelect: function (formId, name, value) {//radio选中
        var _o = document.forms[formId][name];
        for (i = 0; i < _o.length; i++) {
            if (_o[i].value == String(value))
            { _o[i].checked = true; }
            else {
                _o[i].checked = false;
            }
        }
    },
    RadioBindSelect: function (formId, name, value) {//radio绑定控件
        var _o = document.forms[formId][name];
        for (i = 0; i < _o.length; i++) {
            if (_o[i].value == String(value))
            { _o[i].checked = true; }
            else {
                _o[i].checked = false;
            }
        }
    },
    RadioGetSelectValue: function (formId, name) {//radio获取选中值
        var _o = document.forms[formId][name];
        for (i = 0; i < _o.length; i++) {
            if (_o[i].checked) {
                return _o[i].value;
                break;
            }
        }
    }

}


//设置宽高自适应w  h 宽高偏移量
function PanelAutoResizeWH(PanelSelect, w, h) {
    var winWidth = GetwinWidth();
    var winHeight = GetwinHeight();

    var ww = winWidth - w;
    var hh = winHeight - h;
    $(PanelSelect).window('resize', {
        width: ww,
        height: hh
    });


}
//设置宽高自适应w  h 宽高偏移量
function AutoWH(PanelSelect, w, h) {
    var winWidth = GetwinWidth();
    var winHeight = GetwinHeight();

    var ww = winWidth - w;
    var hh = winHeight - h;
    $(PanelSelect).width(ww);
    $(PanelSelect).height(hh);

}
//设置宽自适应w  宽偏移量
function AutoW(PanelSelect, w) //设置宽自适应w宽高偏移量
{
    //获取窗口宽度
    var winWidth = GetwinWidth();
    var ww = winWidth - w;
    $(PanelSelect).width(ww);
}

//设置高自适应 h 高偏移量
function AutoH(PanelSelect, h) //设置高自适应 h 宽高偏移量
{

    var winHeight = GetwinHeight();

    var hh = winHeight - h;
    $(PanelSelect).height(hh);
}

//当高度》（浏览器高度值减去偏移量），设置高自适应 ，h 宽高偏移量
function AutoMaxH(PanelSelect, h) {
    //获取窗口高度
    var winHeight = GetwinHeight();
    var hh = winHeight - h;
    if ($(PanelSelect).height() > hh) {
        $(PanelSelect).height(hh);
    }
}

//获取窗口高度
function GetwinHeight() {
    var winHeight = 0;
    //获取窗口高度
    if (window.innerHeight)
        winHeight = window.innerHeight;
    else if ((document.body) && (document.body.clientHeight))
        winHeight = document.body.clientHeight;
    //通过深入Document内部对body进行检测，获取窗口大小
    if (document.documentElement && document.documentElement.clientHeight) {
        winHeight = document.documentElement.clientHeight;
    }
    return winHeight;
}
//获取窗口宽度
function GetwinWidth() {
    var winWidth = 0;
    //获取窗口宽度
    if (window.innerWidth)
        winWidth = window.innerWidth;
    else if ((document.body) && (document.body.clientWidth))
        winWidth = document.body.clientWidth;
    //通过深入Document内部对body进行检测，获取窗口大小
    if (document.documentElement && document.documentElement.clientWidth) {
        winWidth = document.documentElement.clientWidth;
    }
    return winWidth;

}



//JS获取地址栏参数的方法（超级简单）
function GetQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]); return null;
}


var easyUIHelper = {
    //只显示年月
    dateboxShowMonth: function (dateboxSelector) {
        $(dateboxSelector).datebox({
            onShowPanel: function () {//显示日趋选择对象后再触发弹出月份层的事件，初始化时没有生成月份层
                span.trigger('click'); //触发click事件弹出月份层
                if (!tds) setTimeout(function () {//延时触发获取月份对象，因为上面的事件触发和对象生成有时间间隔
                    tds = p.find('div.calendar-menu-month-inner td');
                    tds.click(function (e) {
                        e.stopPropagation(); //禁止冒泡执行easyui给月份绑定的事件
                        var year = /\d{4}/.exec(span.html())[0]//得到年份
                        , month = parseInt($(this).attr('abbr'), 10); //月份，这里不需要+1
                        $(dateboxSelector).datebox('hidePanel')//隐藏日期对象
                        .datebox('setValue', year + '-' + month); //设置日期的值
                    });
                }, 0)
            },
            parser: function (s) {
                if (!s) return new Date();
                var arr = s.split('-');
                return new Date(parseInt(arr[0], 10), parseInt(arr[1], 10) - 1, 1);
            },
            formatter: function (d) { return d.getFullYear() + '-' + (d.getMonth() + 1);/*getMonth返回的是0开始的，忘记了。。已修正*/ }
        });
        var p = $(dateboxSelector).datebox('panel'), //日期选择对象
            tds = false, //日期选择对象中月份
            span = p.find('span.calendar-text'); //显示月份层的触发控件

    }


}