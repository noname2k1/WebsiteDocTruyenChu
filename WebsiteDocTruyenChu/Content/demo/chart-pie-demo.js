import { clearCanvas } from './adminPrimary.js'
// Set new default font family and font color to mimic Bootstrap's default styling
Chart.defaults.global.defaultFontFamily = 'Nunito', '-apple-system,system-ui,BlinkMacSystemFont,"Segoe UI",Roboto,"Helvetica Neue",Arial,sans-serif';
Chart.defaults.global.defaultFontColor = '#858796';

// Pie Chart Example
//const ctx = document.getElementById("myPieChart");
const backgroundColor = [];
const hoverBackgroundColor = [];
const data = [];
const labels = [];
const formData = new FormData()

export const loadPieChart = async () => {
    const pieChartStatus = document.querySelector(".chart-pie-status");
    if (document.body.dataset.reload) {
        pieChartStatus.textContent = "Reject";
        return;
    }
    pieChartStatus.textContent = "Loading...";
    const getCategories = await fetch("/Service/GetCategories");
    const resCategories = await getCategories.json();
    if (resCategories.Data.Success) {
        //console.log(resCategories.Data.data)
        if (document.body.dataset.reload) {
            document.querySelector(".chart-pie-status").textContent = "Reject";
            return;
        }
        const categories = resCategories.Data.data;
        const categoryLabels = categories.map(category => category.categoryName);
        for (let i = 0; i < categories.length; i++) {
            if (document.body.dataset.reload) {
                document.querySelector(".chart-pie-status").textContent = "Reject";
                return;
            }
            if (formData.has("categorySlug")) {
                formData.set("categorySlug", categories[i].path)
            } else {
                formData.append("categorySlug", categories[i].path)
            }
            const getStoryCountByCategory = await fetch("/Service/GetStoryCountByCategory", {
                method: "POST",
                body: formData
            })
            const resStoryCountByCategory = await getStoryCountByCategory.json()
            if (resStoryCountByCategory.Data.Success) {
                data.push(resStoryCountByCategory.Data.data)
                const bgColor = getRandomHexColor();
                backgroundColor.push(bgColor)
                hoverBackgroundColor.push(getRandomHexColor())
                labels.push(categories[i].categoryName)
                clearCanvas(".chart-pie", "#myPieChart", "id")
                renderPieChart("#myPieChart")
                const labelElement = document.createElement("span");
                labelElement.className = "mr-2";
                labelElement.innerHTML = `<i class="fas fa-circle" style="color:${bgColor};"></i><span class="ml-1">${categories[i].categoryName}</span>`;
                document.querySelector(".chart-pie-label-list").append(labelElement);
            }
        }
    } else {
        // error
    }
    document.querySelector(".chart-pie-status").textContent = "Ready";
}

function getRandomHexColor() {
    // Tạo giá trị ngẫu nhiên cho các thành phần màu
    const red = Math.floor(Math.random() * 256);
    const green = Math.floor(Math.random() * 256);
    const blue = Math.floor(Math.random() * 256);
    // Tạo mã màu hex từ các giá trị thành phần màu
    const hexColor = '#' + red.toString(16).padStart(2, '0') + green.toString(16).padStart(2, '0') + blue.toString(16).padStart(2, '0');
    return hexColor;
}

const renderPieChart = (context) => {
    const ctx = document.querySelector(context);
    var myPieChart = new Chart(ctx, {
        type: 'doughnut',
        data: {
            labels,
            datasets: [{
                data,
                backgroundColor,
                hoverBackgroundColor,
                hoverBorderColor: "rgba(234, 236, 244, 1)",
            }],
        },
        options: {
            maintainAspectRatio: false,
            tooltips: {
                backgroundColor: "rgb(255,255,255)",
                bodyFontColor: "#858796",
                borderColor: '#dddfeb',
                borderWidth: 1,
                xPadding: 15,
                yPadding: 15,
                displayColors: false,
                caretPadding: 10,
            },
            legend: {
                display: false
            },
            cutoutPercentage: 80,
        },
    });
}

