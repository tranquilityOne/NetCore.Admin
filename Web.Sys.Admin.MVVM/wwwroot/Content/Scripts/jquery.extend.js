(function ($) {
    $.extend($,
        {
            //显示消息：如果有easyui，则调用easyui的message组件显示消息
            alertMsg: function (msg, title, funcSuc) {
                //error,question,info,warning
                if ($.messager) {
                    $.messager.alert(title, msg, "info", function () {
                        if (funcSuc) funcSuc();
                    });
                } else {
                    alert(title + "\r\n     " + msg);
                    if (funcSuc) funcSuc();
                }
            },
            //统一处理 返回的json数据格式
            procAjaxData: function (data, funcSuc, funcErr) {
                if (!data || !data.Statu) {
                    return;
                }

                switch (data.Statu) {
                    case "ok":
                        if (data.Msg && data.Msg.trim() != "") {

                            $.alertMsg(data.Msg, "系统提示", function () {
                                if (funcSuc != null && funcSuc != undefined) {
                                    funcSuc(data.Data);
                                }

                            });
                        }
                        else
                            if (funcSuc != null && funcSuc != undefined) {
                                funcSuc(data.Data);
                            }
                        break;
                    case "err":
                        if (data.Msg && data.Msg.trim() != "") {
                            $.alertMsg(data.Msg, "系统提示", function () {
                                if (funcErr != null && funcErr != undefined) {
                                    funcErr(data.Data);
                                }
                            });
                        }
                        else
                            if (funcErr != null && funcErr != undefined) {
                                funcErr(data.Data);
                            }
                        break;
                    case "nologin":
                        $.alertMsg(data.Msg, "系统提示", function () { if (window.top) window.top.location = data.BackUrl; else window.location = data.BackUrl; });
                        break;
                }
            },
            ajaxPost: function (url, Data, onSuccess, OnError) {

                $.ajax({
                    type: "POST", //访问WebService使用Post方式请求
                    contentType: "application/json;charset=utf-8", //WebService 会返回Json类型
                    url: url, //调用WebService
                    data: Data, //Email参数
                    dataType: 'json',
                    beforeSend: function (x) { x.setRequestHeader("Content-Type", "application/json; charset=utf-8"); },
                    error: function (x, e) {
                        if (OnError != null) {
                            OnError();
                        }
                    },
                    success: function (data) { //回调函数，result，返回值
                        if (onSuccess != null) {
                            onSuccess(data);
                        }
                    }
                });

            },
            ajaxGet: function (url, Data, onSuccess, OnError) {

                $.ajax({
                    type: "GET", //访问WebService使用Post方式请求
                    contentType: "application/json;charset=utf-8", //WebService 会返回Json类型
                    url: url, //调用WebService
                    data: Data, //Email参数
                    dataType: 'json',
                    beforeSend: function (x) { x.setRequestHeader("Content-Type", "application/json; charset=utf-8"); },
                    error: function (x, e) {
                        if (OnError != null) {
                            OnError(e, e);
                        }
                    },
                    success: function (data) { //回调函数，result，返回值
                        if (onSuccess != null) {
                            onSuccess(data);
                        }
                    }
                });

            },
            ajaxCrossGet: function (url, onSuccess, OnError) {
                $.ajax({
                    type: "get",
                    async: false,
                    url: url,
                    dataType: "jsonp",
                    jsonp: "callbackparam",//服务端用于接收callback调用的function名的参数
                    jsonpCallback: "success_jsonpCallback",//callback的function名称
                    success: function (json) {
                        if (onSuccess != null) {
                            onSuccess(json);
                        }
                    },
                    error: function (x, e) {
                        if (OnError != null) {
                            OnError(e, e);
                        }
                    }
                });
            },
            baiduAddressParse: function (lat, lng, onSuccess, OnError) {
                var url = 'http://112.74.204.63:8081/baidu_address_parse_cross?lat=' + lat + '&lng=' + lng;
                $.ajaxCrossGet(url, onSuccess, OnError);
            },
            isArray: function (o) {
                return Object.prototype.toString.call(o) === "[object Array]";
            },

            /**
             * 判断是否为函数
             * @param {Object} o
             * @return {Boolean}
             */
            isFunction: function (o) {
                return Object.prototype.toString.call(o) === "[object Function]";
            },

            /**
             * 判断是否为数值。NaN，正负无穷大为false
             * @param {Object} o
             * @return {Boolean}
             */
            isNumber: function (o) {
                return Object.prototype.toString.call(o) === "[object Number]" && isFinite(o);
            },

            /**
             * 判断是否为对象
             * @param {Object} o
             * @return {Boolean}
             */
            isObject: function (o) {
                return typeof o === "function" || !!(o && typeof o === "object");
            },

            /**
             * 判断是否为字符串
             * @param {Object} o
             * @return {Boolean}
             */
            isString: function (o) {
                return Object.prototype.toString.call(o) === "[object String]";
            }

        });
}(jQuery));