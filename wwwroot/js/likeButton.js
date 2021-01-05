function likeB() {
    var cookies = document.cookie; //全てのcookieを取り出して
    var cookiesArray = cookies.split(';'); // ;で分割し配列に

    var pattern = /[0-9]+liked/g;
    var likedArray = [];

    for (var i = 0; i < cookiesArray.length; i++) { //一つ一つ取り出して
        var cArray = cookiesArray[i].split('='); //さらに=で分割して配列に
        var result = cArray[0].match(pattern);
        if (result) { // 取り出したいkeyと合致したら
            console.log(result);  // [key,value] 
            likedArray.push(cArray[1] + "likeButton");
        }
    }
    console.log(likedArray);
    for (var i = 0; i < likedArray.length; i++) {
        var formElem = document.getElementById(likedArray[i]);
        formElem.disabled = true;
        formElem.style.backgroundColor = "#555555";
    }
}