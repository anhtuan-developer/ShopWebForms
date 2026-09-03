(function () {
    "use strict";

    function initChatbot() {

        var launcher =
            document.getElementById(
                "aiChatbotLauncher");

        var windowEl =
            document.getElementById(
                "aiChatbotWindow");

        var closeButton =
            document.getElementById(
                "aiChatbotClose");

        var form =
            document.getElementById(
                "aiChatbotForm");

        var input =
            document.getElementById(
                "aiChatbotInput");

        var messages =
            document.getElementById(
                "aiChatbotMessages");

        var sendButton =
            document.getElementById(
                "aiChatbotSend");

        var endpoint =
            windowEl
                ? windowEl.getAttribute(
                    "data-endpoint")
                : null;

        if (
            !launcher ||
            !windowEl ||
            !input ||
            !messages ||
            !endpoint
        ) {
            return;
        }

        function openChat() {

            windowEl.classList.add(
                "is-open");

            launcher.setAttribute(
                "aria-expanded",
                "true");

            input.focus();
        }

        function closeChat() {

            windowEl.classList.remove(
                "is-open");

            launcher.setAttribute(
                "aria-expanded",
                "false");
        }

        function scrollBottom() {

            messages.scrollTop =
                messages.scrollHeight;
        }

        function addMessage(
            text,
            type) {

            var row =
                document.createElement(
                    "div");

            row.className =
                "ai-chatbot-message "
                + type;

            var bubble =
                document.createElement(
                    "div");

            bubble.className =
                "ai-chatbot-bubble";

            bubble.textContent =
                text || "";

            row.appendChild(
                bubble);

            messages.appendChild(
                row);

            scrollBottom();

            return row;
        }

        function addTyping() {

            var row =
                document.createElement(
                    "div");

            row.className =
                "ai-chatbot-message bot";

            row.id =
                "aiChatbotTyping";

            var bubble =
                document.createElement(
                    "div");

            bubble.className =
                "ai-chatbot-bubble";

            bubble.innerHTML =
                '<span class="ai-chatbot-typing">' +
                '<span></span>' +
                '<span></span>' +
                '<span></span>' +
                '</span>';

            row.appendChild(
                bubble);

            messages.appendChild(
                row);

            scrollBottom();

            return row;
        }

        function sendMessage() {

            var message =
                (input.value || "")
                    .trim();

            if (
                !message ||
                sendButton.disabled
            ) {
                return;
            }

            addMessage(
                message,
                "user");

            input.value = "";

            input.disabled = true;

            sendButton.disabled = true;

            var typing =
                addTyping();

            fetch(
                endpoint,
                {
                    method: "POST",

                    credentials:
                        "same-origin",

                    headers: {
                        "Content-Type":
                            "application/json; charset=UTF-8",

                        "Accept":
                            "application/json"
                    },

                    body: JSON.stringify({
                        message: message
                    })
                })

                .then(function (response) {

                    return response.text()
                        .then(function (text) {

                            var data;

                            try {

                                data =
                                    JSON.parse(
                                        text);
                            }
                            catch (e) {

                                console.error(
                                    "Chatbot raw response:",
                                    text);

                                throw new Error(
                                    "Server chatbot trả về dữ liệu không hợp lệ "
                                    + "(HTTP "
                                    + response.status
                                    + ").");
                            }

                            if (
                                !response.ok ||
                                !data.success
                            ) {

                                throw new Error(
                                    data.message ||
                                    "Chatbot server lỗi HTTP "
                                    + response.status
                                    + ".");
                            }

                            return data;
                        });
                })

                .then(function (data) {

                    if (
                        typing &&
                        typing.parentNode
                    ) {

                        typing.parentNode
                            .removeChild(
                                typing);
                    }

                    addMessage(
                        data.message,
                        "bot");
                })

                .catch(function (error) {

                    if (
                        typing &&
                        typing.parentNode
                    ) {

                        typing.parentNode
                            .removeChild(
                                typing);
                    }

                    addMessage(
                        error.message ||
                        "Xin lỗi, chatbot đang "
                        + "gặp sự cố.",
                        "bot");
                })

                .finally(function () {

                    input.disabled =
                        false;

                    sendButton.disabled =
                        false;

                    input.focus();
                });
        }

        launcher.addEventListener(
            "click",
            function () {

                if (
                    windowEl.classList.contains(
                        "is-open")
                ) {

                    closeChat();

                }
                else {

                    openChat();
                }
            });

        if (closeButton) {

            closeButton.addEventListener(
                "click",
                closeChat);
        }

        sendButton.addEventListener(
            "click",
            function () {
                sendMessage();
            });

        input.addEventListener(
            "keydown",
            function (event) {

                if (
                    event.key === "Enter" &&
                    !event.shiftKey
                ) {

                    event.preventDefault();

                    sendMessage();
                }
            });

        document.addEventListener(
            "keydown",
            function (event) {

                if (event.key === "Escape") {

                    closeChat();
                }
            });
    }

    if (
        document.readyState ===
        "loading"
    ) {

        document.addEventListener(
            "DOMContentLoaded",
            initChatbot);

    }
    else {

        initChatbot();
    }

})();