document.addEventListener("DOMContentLoaded", function () {


    // ==========================
    // ELEMENT
    // ==========================


    const editPopup = document.getElementById("editPopup");

    const passwordPopup = document.getElementById("passwordPopup");


    const openEdit = document.getElementById("openEdit");

    const openPassword = document.getElementById("openPassword");



    const closeEdit = document.getElementById("closeEdit");

    const cancelEdit = document.getElementById("cancelEdit");


    const closePassword = document.getElementById("closePassword");

    const cancelPassword = document.getElementById("cancelPassword");



    const saveInfo = document.getElementById("saveInfo");

    const savePassword = document.getElementById("savePassword");





    // ==========================
    // MỞ POPUP
    // ==========================


    openEdit.onclick = function () {

        editPopup.classList.add("show");

    }



    openPassword.onclick = function () {

        passwordPopup.classList.add("show");

    }





    // ==========================
    // ĐÓNG POPUP
    // ==========================


    closeEdit.onclick =
        cancelEdit.onclick = function () {

            editPopup.classList.remove("show");

        }




    closePassword.onclick =
        cancelPassword.onclick = function () {

            passwordPopup.classList.remove("show");

        }





    // click nền ngoài popup

    window.onclick = function (e) {


        if (e.target === editPopup) {

            editPopup.classList.remove("show");

        }


        if (e.target === passwordPopup) {

            passwordPopup.classList.remove("show");

        }


    }





    // ==========================
    // UPDATE THÔNG TIN
    // ==========================


    saveInfo.onclick = function () {

        document.getElementById("editMessage").innerText = "";


        let data = new URLSearchParams();


        data.append(
            "HoTen",
            document.getElementById("editHoTen").value
        );


        data.append(
            "Email",
            document.getElementById("editEmail").value
        );


        data.append(
            "SDT",
            document.getElementById("editSDT").value
        );


        data.append(
            "DiaChi",
            document.getElementById("editDiaChi").value
        );





        saveInfo.innerHTML =
            "Đang lưu...";

        saveInfo.disabled = true;





        fetch("/ThongTin/UpdateInfo",
            {

                method: "POST",

                headers:
                {
                    "Content-Type":
                        "application/x-www-form-urlencoded"
                },

                body: data

            })

            .then(res => res.json())

            .then(result => {



                if (result.success) {

                    document.getElementById("showHoTen").innerText =
                        document.getElementById("editHoTen").value;

                    document.getElementById("showEmail").innerText =
                        document.getElementById("editEmail").value;

                    document.getElementById("showSDT").innerText =
                        document.getElementById("editSDT").value;

                    document.getElementById("showDiaChi").innerText =
                        document.getElementById("editDiaChi").value;

                    editPopup.classList.remove("show");
                }
                else {

                    const msg = document.getElementById("editMessage");

                    msg.innerText = result.message;
                    msg.style.color = "#dc3545";
                    msg.style.display = "block";

                }
            })

            .catch(error => {


                alert(
                    "Có lỗi xảy ra!"
                );


                console.log(error);


            })

            .finally(() => {


                saveInfo.innerHTML =
                    '<i class="fa-solid fa-floppy-disk"></i> Lưu thay đổi';


                saveInfo.disabled = false;


            });



    }





    // ==========================
    // ĐỔI MẬT KHẨU
    // ==========================


    savePassword.onclick = function () {



        let data = new URLSearchParams();



        data.append(
            "MatKhauCu",
            document.getElementById("oldPassword").value
        );



        data.append(
            "MatKhauMoi",
            document.getElementById("newPassword").value
        );



        data.append(
            "NhapLai",
            document.getElementById("confirmPassword").value
        );






        savePassword.innerText =
            "Đang xử lý...";

        savePassword.disabled = true;






        fetch("/ThongTin/ChangePassword",
            {

                method: "POST",

                headers:
                {
                    "Content-Type":
                        "application/x-www-form-urlencoded"
                },


                body: data


            })

            .then(res => res.json())
            .then(result => {

                const msg = document.getElementById("passwordMessage");

                if (result.success) {

                    msg.innerText = result.message;
                    msg.style.color = "#198754";
                    msg.style.display = "block";

                    document.getElementById("oldPassword").value = "";
                    document.getElementById("newPassword").value = "";
                    document.getElementById("confirmPassword").value = "";

                    setTimeout(() => {

                        passwordPopup.classList.remove("show");
                        msg.innerText = "";
                        msg.style.display = "none";

                    }, 1000);

                }
                else {

                    msg.innerText = result.message;
                    msg.style.color = "#dc3545";
                    msg.style.display = "block";

                }

            })
            .catch(error => {


                console.log(error);


                alert(
                    "Lỗi kết nối server"
                );


            })


            .finally(() => {


                savePassword.innerText =
                    "Đổi mật khẩu";


                savePassword.disabled = false;


            });
    }
    document.querySelectorAll(".toggle-password").forEach(icon => {

        icon.addEventListener("click", function () {

            const input = document.getElementById(this.dataset.target);

            if (input.type === "password") {

                input.type = "text";
                this.classList.remove("fa-eye");
                this.classList.add("fa-eye-slash");

            } else {

                input.type = "password";
                this.classList.remove("fa-eye-slash");
                this.classList.add("fa-eye");

            }

        });


    });

});