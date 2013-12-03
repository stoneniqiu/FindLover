// code written by Mark Holton
// MIT license
// initial release 12/15/2010
// https://github.com/holtonma/infini_scroll (soon to be public repo)
// version 0.1
// requires jQuery (I wrote it with 1.4, but it likely works with earlier versions, 
// will check into that later)
// if you re-use, ask that you keep this in the header
// http://markholton.com

// USAGE: 
/*
  $('#posts').infiniScroll();   // initialize
  $('#posts').infiniScroll({ // calls the init method overrides defaults
    'interval' : 200
    ,'root_url' : '/my_posts'
    ,'loading_elem': 'loading'
    ,'data_elem': 'leaderboard'
    ,'num' : 12
  });
*/

(function( $ ){
  
  var _checkLevel = function( ) { 
    // if it's low enough, grab latest data
    if (!_levelReached()){
      return methods.pollLevel();
    } else {
      if (settings.freshest_id > settings.lowest_id){
        $('#'+settings.loading_elem).show();
        $.ajax({
          type: "GET",
          url: settings.root_url+"?id="+settings.freshest_id+"&n="+settings.num,
          dataType: 'html',
          timeout: 3000,
          success: function(data) {
            $('#'+settings.data_elem).append(data);
              $('#'+settings.loading_elem).hide();
              settings.freshest_id = parseInt( $('div.post').last()[0].id.split("_")[1] );
              methods.pollLevel()
            } 
          });
        }  // do not make ajax request if it's the only one left
      } // check if it's low enough 
  };
  
  var _setLastID = function(elem, lastID){
    elem.data("lastID", lastID);
  };

  var _levelReached = function(){
    // is it low enough to add elements to bottom?
    var pageHeight = Math.max(document.body.scrollHeight ||
      document.body.offsetHeight);
    var viewportHeight = window.innerHeight  ||
      document.documentElement.clientHeight  ||
      document.body.clientHeight || 0;
    var scrollHeight = window.pageYOffset ||
      document.documentElement.scrollTop  ||
      document.body.scrollTop || 0;
    // Trigger for scrolls within 20 pixels from page bottom
    return pageHeight - viewportHeight - scrollHeight < 30;
  };

  /* PUBLIC METHODS */
  var methods = {
    init : function( options ) { 
      $('#'+settings.loading_elem).hide();
    },

    pollLevel : function() { 
      // checking every so often:  
      setTimeout(_checkLevel, 100);
    }   
  };
  
  var settings = {
     'interval'     : 100
    ,'root_url'     : '/grab_next'
    ,'loading_elem' : 'spinner'
    ,'data_elem'    : 'posts'
    ,'freshest_id'  : 1000000
    ,'num'          : 5
    ,'lowest_id'    : 1
    ,'sort'         : 'desc'
  };

  $.fn.infiniScroll = function(method) {
    // Method calling logic
    if ( methods[method] ) {
      return methods[ method ].apply( this, Array.prototype.slice.call( arguments, 1 ));
    } else if ( typeof method === 'object' || ! method ) {
      return methods.init.apply( this, arguments );
    } else {
      $.error( 'Method ' +  method + ' does not exist on jQuery.infiniScroll' );
    }  
  }

})( jQuery );