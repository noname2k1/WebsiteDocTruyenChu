import { loadAreaChart } from "./chart-area-demo.js";
import { loadPieChart } from "./chart-pie-demo.js";
export function clearCanvas(parentSelect, targetSelector, attributeType) {
    const targetElement = document.querySelector(targetSelector);
    targetElement.remove();
    // Tạo phần tử canvas mới
    const newCanvas = document.createElement("canvas");
    // Thiết lập thuộc tính và nội dung cho phần tử
    newCanvas[attributeType] = targetSelector.slice(1);
    // Thêm phần tử vào cây DOM
    const parentElement = document.querySelector(parentSelect);
    parentElement.appendChild(newCanvas);
}

window.addEventListener("beforeunload", () => {
    document.body.dataset.reload = true;
})

$(function () {
    $('[data-toggle="tooltip"]').tooltip()
})

Promise.all([loadPieChart(), loadAreaChart()]);
