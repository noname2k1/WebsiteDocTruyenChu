$(document).ready(function () {
    const searchStory = $('.search-story')
    if (searchStory) {
        searchStory.on('keyup', function (e) {
            /* console.log($(this).val());*/
            const searchResult = $('.search-result')
            const list = searchResult.find('.list-group')

            if ($(this).val().length == 0) {
                if (searchResult) {
                    searchResult.addClass('d-none')
                    searchResult.addClass('no-result')
                    list.addClass('d-none')
                }
            } else {
                function debounce(func, delay) {
                    let timerId;

                    return function (...args) {
                        clearTimeout(timerId);

                        timerId = setTimeout(() => {
                            func.apply(this, args);
                        }, delay);
                    };
                }

                function fetchData(searchTerm) {
                    // Gọi API fetch ở đây và xử lý dữ liệu trả về
                    fetch('/tim-kiem/' + searchTerm, {
                        method: 'GET',
                        headers: {
                            'Accept': 'application/json',
                            'Content-Type': 'application/json'
                        },
                    })
                        .then(res => res.json())
                        .then(data => {
                            //console.log(data);
                            if (data.success) {
                                let html = ''
                                if (searchResult) {
                                    searchResult.removeClass('d-none')
                                    list.empty()
                                    searchResult.removeClass('no-result')
                                    list.removeClass('d-none')

                                    if (data.html.length) {
                                        list.html(data.html)
                                    } else {
                                        html += `
                                    <li class="list-group-item border-0">
                                    Không tìm thấy truyện nào 
                                    </li>
                                    `;
                                        list.html(html);
                                    }

                                }
                            }
                        })
                        .catch(function (error) {
                            console.log(error);
                            if (error.status !== 500) {
                                let errorMessages = error.responseJSON.errors;
                            } else {
                                errorContent = error.responseJSON.message;
                            }
                        })
                }

                const debouncedFetchData = debounce(fetchData, 1000); // Thiết lập debounce cho hàm fetchData với khoảng thời gian delay là 300ms

                // Sử dụng hàm debouncedFetchData khi cần gọi API fetch
                debouncedFetchData($(this).val());
            }
        })
    }
})