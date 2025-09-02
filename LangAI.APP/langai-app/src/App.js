import "./App.css";
import { BrowserRouter, Routes, Route, useLocation } from "react-router-dom";
import { Home } from "./pages/Home/Home";
import Perfil from "./pages/Perfil/Perfil";
import { Login } from "./pages/Login/Login";
import { Cadastro } from "./pages/Cadastro/Cadastro";
import { ToastContainer } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import { RotaProtegida } from "./components/RotaProtegida/RotaProtegida";
import { UsuariosPage } from "./components/UsuariosPage/UsuariosPage";
import Exercicio from "./pages/Exercicio/Exercicio";
import Idioma from "./pages/Idioma/Idioma";
import Ranking from "./pages/Ranking/Ranking";
import { PainelAdm } from "./components/PainelAdm/PainelAdm";
import { AnimatePresence } from "framer-motion";
import { useEffect } from "react";
import { RequireAdmin } from "./components/RequireAdmin/RequireAdmin";
import Progresso from "./pages/Progresso/Progresso";

function App() {
  return (
    <BrowserRouter>
      <ToastContainer
        position="top-right"
        autoClose={3000}
        hideProgressBar={false}
        closeOnClick
        pauseOnHover
        theme="colored"
      />
      <AppRoutes />
    </BrowserRouter>
  );
}

function AppRoutes() {
  const location = useLocation();
  const isAuthPage = ["/login", "/cadastro"].includes(location.pathname);

  // Rolagem para o topo ao mudar de rota
  useEffect(() => {
    window.scrollTo(0, 0);
  }, [location.pathname]);

  return (
    <>
      {isAuthPage ? (
        <AnimatePresence mode="wait">
          <Routes location={location} key={location.pathname}>
            <Route path="/login" element={<Login />} />
            <Route path="/cadastro" element={<Cadastro />} />
          </Routes>
        </AnimatePresence>
      ) : (
        <Routes>
          <Route path="/home" element={<Home />} />
          <Route
            path="/perfil"
            element={
              <RotaProtegida>
                <Perfil />
              </RotaProtegida>
            }
          />
          <Route path="/idioma" element={<Idioma />} />
          <Route path="/exercicio" element={<Exercicio />} />
          <Route path="/progresso" element={<Progresso />} />

          <Route
            path="/usuarios"
            element={
              <RotaProtegida>
                <UsuariosPage />
              </RotaProtegida>
            }
          />
          <Route path="/ranking" element={<Ranking />} />
          <Route
            path="/admin"
            element={
              <RequireAdmin>
                <PainelAdm />
              </RequireAdmin>
            }
          />
        </Routes>
      )}
    </>
  );
}

export default App;
