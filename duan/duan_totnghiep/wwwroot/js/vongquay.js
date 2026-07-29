const svg = document.getElementById("wheelSvg");
const btnSpin = document.getElementById("btnSpin");

const popup = document.getElementById("popup");
const giftName = document.getElementById("giftName");
const closePopup = document.getElementById("closePopup");

const SIZE = 700;
const CENTER = 350;
const RADIUS = 300;

const COLORS = [
    "#ef4444",
    "#f97316",
    "#f59e0b",
    "#10b981",
    "#06b6d4",
    "#3b82f6",
    "#8b5cf6",
    "#ec4899"
];

let currentRotate = 0;
let spinning = false;

drawWheel();

btnSpin.onclick = spin;

function polar(cx, cy, r, deg) {

    const rad = (deg - 90) * Math.PI / 180;

    return {
        x: cx + r * Math.cos(rad),
        y: cy + r * Math.sin(rad)
    };
}

function drawWheel() {

    svg.innerHTML = "";

    const total = vouchers.length;

    if (total == 0)
        return;

    const angle = 360 / total;

    vouchers.forEach((v, i) => {

        const start = i * angle;
        const end = start + angle;

        const p1 = polar(CENTER, CENTER, RADIUS, start);
        const p2 = polar(CENTER, CENTER, RADIUS, end);

        const path = document.createElementNS(
            "http://www.w3.org/2000/svg",
            "path"
        );

        path.setAttribute(
            "d",
            `
            M ${CENTER} ${CENTER}
            L ${p1.x} ${p1.y}
            A ${RADIUS} ${RADIUS} 0 0 1 ${p2.x} ${p2.y}
            Z
            `
        );

        path.setAttribute("fill", COLORS[i % COLORS.length]);

        svg.appendChild(path);

        //------------------ TEXT ------------------

        const mid = start + angle / 2;

        const tp = polar(
            CENTER,
            CENTER,
            RADIUS * 0.68,
            mid
        );

        const text = document.createElementNS(
            "http://www.w3.org/2000/svg",
            "text"
        );

        text.setAttribute("x", tp.x);
        text.setAttribute("y", tp.y);

        text.setAttribute("fill", "white");
        text.setAttribute("font-size", "20");
        text.setAttribute("font-weight", "700");
        text.setAttribute("text-anchor", "middle");

        text.setAttribute(
            "transform",
            `rotate(${mid + 90},${tp.x},${tp.y})`
        );

        text.textContent = v.ten;

        svg.appendChild(text);

    });

}

async function spin() {

    if (spinning)
        return;

    spinning = true;

    btnSpin.disabled = true;

    try {

        const response = await fetch("/Trangmua/Quay", {
            method: "POST"
        });

        const data = await response.json();

        if (!data.success) {

            alert(data.message);

            spinning = false;

            btnSpin.disabled = false;

            return;
        }

        const index = vouchers.findIndex(x => x.id == data.makm);

        if (index == -1) {

            alert("Voucher không tồn tại.");

            spinning = false;

            btnSpin.disabled = false;

            return;
        }

        const angle = 360 / vouchers.length;

        const target = index * angle + angle / 2;

        const stop = 360 - target;

        currentRotate += 360 * 8 + stop;

        svg.style.transform = `rotate(${currentRotate}deg)`;

        svg.ontransitionend = function () {

            giftName.innerHTML =
                `
                ${data.ten}
                <br>
                <span style="font-size:22px">
                    Giảm ${data.giam}%
                </span>
                `;

            popup.classList.add("show");

            spinning = false;

            btnSpin.disabled = false;

        };

    }
    catch (err) {

        console.log(err);

        alert("Có lỗi xảy ra.");

        spinning = false;

        btnSpin.disabled = false;

    }

}

closePopup.onclick = function () {

    popup.classList.remove("show");

}

window.onclick = function (e) {

    if (e.target == popup)
        popup.classList.remove("show");

}

window.onkeydown = function (e) {

    if (e.key === "Escape")
        popup.classList.remove("show");

}