// JavaScript
function displayCartList(show) {
    const cartList = document.getElementById('cartList');
    cartList.classList.toggle('hide', !show);
}

function updateCartList() {

    updateCartItems();
}