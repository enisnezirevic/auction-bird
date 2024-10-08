import React from "react";
import ReactDOM from "react-dom/client";
import App from "./App.tsx";
import "./index.css";

const root: HTMLElement = document.getElementById("root")!;
ReactDOM.createRoot(root).render(
    <React.StrictMode>
        <App/>
    </React.StrictMode>,
);
