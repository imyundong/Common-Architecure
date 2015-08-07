 /*!
 * jQuery Validation Plugin 0.7.5
 *
 *
 * Copyright 2014 Su Jia
 * Released under the MIT license:
 *
 * Date   : 2014/06/15
 * Author : Su Jia
 *
 * Date   : 2014/06/17
 * Author : Su Jia
 * Desc   : Add formatter
 *
 * Date   : 2014/06/22
 * Author : Su Jia
 * Desc   : Select the fileds which has formatter when foucs
 *          Add validation for select/radio
 *
 * Date   : 2014/06/22
 * Author : Su Jia
 * Desc   : Add readonly property, add validate_version variable
 *
 * Date   : 2014/07/10
 * Author : Su Jia
 * Desc   : Auto focus in first err fields
 *
 * Date   : 2015/03/17
 * Author : Su Jia
 * Desc   : Remove auto focus of input button for mobile devices
 */

(function($) {
  $.extend($.fn, {
    val_bind: function() {
      // If nothing is selected, return nothing;
      if (!this.length) return;
      
      var t = $(this)[0].tagName;
      if ($(this).length == 1 && t == "INPUT") {
        $(this).unbind("blur.validation");
        $(this).bind("blur.validation", $.validator.input_validation);
        
        $(this).unbind("keyup.validation");
        $(this).bind("keyup.validation", $.validator.input_validation);
        
        if ($(this).hasClass("required")) { $(this).attr("placeholder", $.validator.messages.required_tip); } else { $(this).attr("placeholder", ""); }
        if ($(this).hasClass("readonly")) { $(this).attr("readonly", true); } else { $(this).attr("readonly", false); }
        
        return;
      }
      
      $(this).find("input, select").attr("class", function(idx, value) {
        $(this).unbind("blur.validation");
        $(this).bind("blur.validation", $.validator.input_validation);
        
        $(this).unbind("keyup.validation");
        $(this).bind("keyup.validation", $.validator.input_validation);
        
        $(this).unbind("focus.validation");
        $(this).bind("focus.validation", $.validator.remove_format);

        $(this).unbind("keydown.nextfocus");
        $(this).bind("keydown.nextfocus", $.validator.next_focus);
      })
      
      
      $(this).find(".required").attr("placeholder", $.validator.messages.required_tip);
      $(this).find(".readonly").attr("readonly", true);
    },
    
    validate: function() {
      // If nothing is selected, return nothing;
      if (!this.length) return true;
      
      var rtn = true;
      var list = $(this).parents("form").find("input, select").attr("class", function (idx, value) {
          if ($.validator.input_validation(null, $(this)[0]) == false) {
              if (rtn == true) $(this)[0].focus();
              rtn = false;
          }
      });
      
      return rtn;
      //$.each(list, function(i, n) { $validator.input_validation() });
    }
    
  })
}(jQuery));

(function($) {
    $.extend({
        isMobile: {
            android: function () {
                return navigator.userAgent.match(/Android/i) ? true : false;
            },
            blackBerry: function () {
                return navigator.userAgent.match(/BlackBerry/i) ? true : false;
            },
            iOS: function () {
                return navigator.userAgent.match(/iPhone|iPad|iPod/i) ? true : false;
            },
            windows: function () {
                return navigator.userAgent.match(/IEMobile/i) ? true : false;
            },
            any: function () {
                return (isMobile.android() || isMobile.blackBerry() || isMobile.iOS() || isMobile.windows());
            }
        },
    validator: {
      remove_format: function (e) {
      	$(this).attr("formatter", function(idx, value) {
            if (e && e.type == "focus") { 
            	if (!$.validator.formatter[value]) return;
              var v = $.validator.formatter[value].restore($(this).val());
              
              if (v) $(this).val(v);
              $(this).select();
            }
        });
      },

      next_focus: function (e, source) {
          if (e.which == 13) {
              list = $(this).parents("form").find(":input");
              source = $(this)[0];
              
              var focus = false;
              $.each(list, function (i, n) {
                  if (focus == true) {
                      if ($(this).attr("type") == "button" && $.isMobile.any()) return true;
                      n.focus();
                      e.preventDefault();
                      return false;
                  }
                  
                  if (source == n) focus = true;
              });
          }
      },

      input_validation: function(e, source) {
        if (source == null) {
          source = $(this)[0];
        }
        // remove validation when get focus
        if (e && e.which && (e.which == 9 || e.which == 13) && e.type == "keyup") return;
        var v = $(source).attr("class");
        if (v == null) return true;
        
        var vl = v.split(" ");
        
        var result = true;
        var option = "";
        var params = {};
        // validate all fields
        for (var idx = 0; idx < vl.length; idx++) {
          option = vl[idx];
          var param = option.split(":");
          if (param.length > 1) { params = param[1].split(","); }
          // get validation option name
          var s = $.validate_option[param[0]];
          
          if (s == null) continue;
          // options[1] => option parameter          
          if ((e && e.type != "keyup") || s.realtime == true || !e) {
          	// in case after click submit, all the formated value should be restored first
          	var sourcevalue = source.value;
          	// special process for radio
          	if (source.type == "radio") {
          		sourcevalue = $("input[name='" + source.name + "']:checked").val();
          		if (!sourcevalue) sourcevalue = "";
          	}
          	
          	if (!e) {
          		$(source).attr("formatter", function(idx, value) {
          			if (value) sourcevalue = $.validator.formatter[value].restore($(source).val());
              });
          	}
          	
            if (s.check(source, sourcevalue, params[0], params[1], params[2]) == false) {
              result = false;
              if (s.realtime == true && s.fix) { source.value = s.fix(source.value) }
              break; 
            }
            else result = true;
          //}
          }
          // 
          // else { return }
        }
        
        // remove the error fields
        $(source).parent().find("[ref=" + $(source).attr("name") + "]").remove();

        if (result) {
          $(source).removeClass("error");
          //if ($(source).next().attr("class")  == "error") $(source).next().remove();
          
          $(source).attr("formatter", function(idx, value) {
              if (e && e.type == "blur") {
                  if (value){
                      var v = $.validator.formatter[value].format(source.value);
                      if (v) source.value = v;
                  }
              }
          });

          return true;
        } else {
          $(source).addClass("error");
          err = $.validator.formatmsg($.validator.messages[param[0]], param[1]);
          if (source.type == "radio") {
          	$("input[name='" + source.name + "']:last").parent().after("<label class='error'>" + err + "</label>");
          } else {
              var errorLabel = "<label ref='" + $(source).attr("name") + "' class='error'>" + err + "</label>"
              if ($(source).next().length > 0 && $(source).next().hasClass("sel_wrap")) {
                  $(source).next().after(errorLabel);
              } else {
                  $(source).after(errorLabel);
              }
              
          }
          
          return false;
        }
      },
      
      messages: {
        required: "This field is required.",
        minlen: "Please enter at least {0} characters.",
        email: "Invalid email address.",
        digit: "Please enter only digits.",
        amount: "Please enter the corrent amount 9(13)V99",
        required_tip: "*Required"
      },
      
      formatter: {
        amount: {
          format: function(value) {
            var newStr = "";
            var count = 0;
            
            if(value.indexOf(".") == -1) {
              for(var i = value.length-1; i >= 0; i--) {
                if(count % 3 == 0 && count != 0){
                  newStr = value.charAt(i) + "," + newStr;
                }else{
                  newStr = value.charAt(i) + newStr;
                }
                count++;
              }
              value = newStr + ".00"; //自动补小数点后两位
            }
            else
            {
              for(var i = value.indexOf(".") - 1; i >= 0; i--) {
                if(count % 3 == 0 && count != 0){
                  newStr = value.charAt(i) + "," + newStr;  //碰到3的倍数则加上“,”号
                }else{
                  newStr = value.charAt(i) + newStr; //逐个字符相接起来
                }
                count++;
              }
              value = newStr + (value + "00").substr((value + "00").indexOf("."),3);
            }
            
            return value;
          },
          
          restore: function(value) {
          	value = value.replace(/,/g, "")
          	return value;
          }
        }
      },
      
      formatmsg: function(source, p) {
        if (p == null) return source;
        
        var params = p.split(",");
        $.each(params, function(i, n) {
          source = source.replace( new RegExp("\\{" + i + "\\}", "g"), function() {
          return n;
          });
        });
        return source;
      }      
    },

    validate_option: {
      required: {
        realtime: true,
        check: function(source, value) { return (value != null && value.length > 0); },
        ime_mode: ""
      },
      digit: {
        realtime: true,
        check: function(source, value) { return $.validate_option.optional.check(source, value) || /^\d*$/.test(value); },
        fix: function(value) { return value.replace(/[^\d]/g, '') }
      },
      email: {
        realtime: false,
        check: function(source, value) { return $.validate_option.optional.check(source, value) || /^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))$/i.test(value) }
      },
      optional: {
        realtime: false,
        check: function(source, value) { return (value == null) || (value.length == 0); }
      },
      amount: {
        realtime: true,
        check: function(source, value) { return $.validate_option.optional.check(source, value) || /^\d{0,13}(?:\.\d{0,2}|)$/.test(value); },
        fix: function(value) {
          var v = value.split(".");
          var s = r = "";
          for (var i = 0; i < v[0].length && i < 13; i++) { if (v[0][i] == " "){ continue; }; if (!isNaN(v[0][i])) r = r + v[0][i].toString(); }
          if (v.length > 1) { for (var i = 0; i < v[1].length && i < 2; i++) { if (v[1][i] == " "){ continue; }; if (!isNaN(v[1][i])) s = s + v[1][i].toString(); }}
          
          return s.length > 0 ? r + "." + s : r;
        }
      },
      minlen: {
        realtime: false,
        check: function(source, value, len) { return (value != null && value.length >= len) }
      }
    },
    
    validate_version: "0.7.1"
  });
}(jQuery));