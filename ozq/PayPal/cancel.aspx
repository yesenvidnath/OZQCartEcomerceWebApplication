<%@ Page Title="" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="cancel.aspx.cs" Inherits="ozq.PayPal.cancel" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div>
        <div class="vh-100 d-flex justify-content-center align-items-center">
            <div class="col-md-4">
                <div class="border border-3 border-danger"></div>
                <div class="card bg-white shadow p-5">
                    <div class="mb-4 text-center">
                        <svg xmlns="http://www.w3.org/2000/svg" class="text-danger" width="75" height="75"
                            fill="currentColor" class="bi bi-x-circle" viewBox="0 0 16 16">
                            <path
                                d="M8 0a8 8 0 1 1 0 16A8 8 0 0 1 8 0zm-.354 4.646a.5.5 0 0 0-.292.293L6.293 8l-2.647 2.647a.5.5 0 1 0 .708.708L8 8.707l2.647 2.647a.5.5 0 0 0 .708-.708L9.293 8l2.647-2.647a.5.5 0 0 0-.708-.708L8 7.293 5.353 4.646a.5.5 0 0 0-.708 0z" />
                        </svg>
                    </div>
                    <div class="text-center">
                        <h1>Payment Canceled 😒</h1>
                        <p>Your payment was canceled.  </p>
                    </div>
                </div>
            </div>
        </div>
    </div>


    <style>
        .box {
            margin-top: 60px;
            display: flex;
            justify-content: space-around;
            flex-wrap: wrap;
        }

        .alert {
            margin-top: 25px;
            background-color: #fff;
            font-size: 25px;
            font-family: sans-serif;
            text-align: center;
            width: 300px;
            height: 100px;
            padding-top: 150px;
            position: relative;
            border: 1px solid #efefda;
            border-radius: 2%;
            box-shadow: 0px 0px 3px 1px #ccc;
        }

            .alert::before {
                width: 100px;
                height: 100px;
                position: absolute;
                border-radius: 100%;
                inset: 20px 0px 0px 100px;
                font-size: 60px;
                line-height: 100px;
                border: 5px solid gray;
                animation-name: reveal;
                animation-duration: 1.5s;
                animation-timing-function: ease-in-out;
            }

            .alert > .alert-body {
                opacity: 0;
                animation-name: reveal-message;
                animation-duration: 1s;
                animation-timing-function: ease-out;
                animation-delay: 1.5s;
                animation-fill-mode: forwards;
            }

        @keyframes reveal-message {
            from {
                opacity: 0;
            }

            to {
                opacity: 1;
            }
        }

        .success {
            color: green;
        }

            .success::before {
                content: '✓';
                background-color: #eff;
                box-shadow: 0px 0px 12px 7px rgba(200,255,150,0.8) inset;
                border: 5px solid green;
            }

        .error {
            color: red;
        }

            .error::before {
                content: '✗';
                background-color: #fef;
                box-shadow: 0px 0px 12px 7px rgba(255,200,150,0.8) inset;
                border: 5px solid red;
            }

        @keyframes reveal {
            0% {
                border: 5px solid transparent;
                color: transparent;
                box-shadow: 0px 0px 12px 7px rgba(255,250,250,0.8) inset;
                transform: rotate(1000deg);
            }

            25% {
                border-top: 5px solid gray;
                color: transparent;
                box-shadow: 0px 0px 17px 10px rgba(255,250,250,0.8) inset;
            }

            50% {
                border-right: 5px solid gray;
                border-left: 5px solid gray;
                color: transparent;
                box-shadow: 0px 0px 17px 10px rgba(200,200,200,0.8) inset;
            }

            75% {
                border-bottom: 5px solid gray;
                color: gray;
                box-shadow: 0px 0px 12px 7px rgba(200,200,200,0.8) inset;
            }

            100% {
                border: 5px solid gray;
                box-shadow: 0px 0px 12px 7px rgba(200,200,200,0.8) inset;
            }
        }

        nav {
            display: none;
        }

        footer {
            display: none;
        }
    </style>
</asp:Content>
