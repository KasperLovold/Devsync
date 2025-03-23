import ReactDOM from "react-dom/client";
import { AuthProvider } from "./context/authcontext";
import AppRouter from "./router";
import { Toaster } from "react-hot-toast";
import "./index.css";

ReactDOM.createRoot(document.getElementById("root")!).render(
  <AuthProvider>
    <Toaster position="top-center" />
    <AppRouter />
  </AuthProvider>,
);
