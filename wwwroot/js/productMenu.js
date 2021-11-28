document.addEventListener("DOMContentLoaded", () => {

    // Change the displayed price when the chosen portion is changed
    // Assisted by https://stackoverflow.com/questions/51573435/want-to-add-addeventlistener-on-multiple-elements-with-same-class
    [...document.querySelectorAll(".product-portion-select")].forEach(select => {
        select.addEventListener("change", (event) => {
            let productId = event.srcElement.getAttribute("product-id");
            let portionIndex = event.srcElement.options[event.srcElement.selectedIndex].getAttribute("portion-index");

            // Hide the outdatedPriceElement, and show the newPriceElement
            let outdatedPriceElement = document.querySelector(`.product-price[product-id='${productId}'][style*='display: block']`);
            let newPriceElement = document.querySelector(`.product-price[product-id='${productId}'][portion-index='${portionIndex}']`);
            outdatedPriceElement.style.display = 'none';
            newPriceElement.style.display = 'block';
        });
    });
});
