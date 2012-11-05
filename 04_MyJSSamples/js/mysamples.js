
// 実行される関数名を設定.
var EXEC_FUNC_NAME = 'alertFunctionSample';

// ready.
$(function(){
  $('#func-name').text(EXEC_FUNC_NAME);
  $('#func-result').text(window[EXEC_FUNC_NAME]());
});


//
// -- Hello World Sample --
//
var helloWorldSample = (function(){
  
  var message = "Hello World Javascript!!";
  
  return function() {
    return message;
  };
}());

//
// -- alert関数のサンプル --
//
var alertFunctionSample = (function(){
  
  var message = "hello javascript!!";
  
  return function(){
    alert(message);
    return;
  };
}());

//
// -- 1から10までSUM
//
var sumSample = (function(){
  
  return function(){
    
    var sum = 0;
    for (var i = 0; i < 10; i++) {
      sum += i;
    }
    
    alert(sum);
    return;
  };
}());
