import "./App.css";
import { BrowserRouter as Router, Route, Routes, useLocation } from "react-router-dom";
import PropertiesPage from "./pages/properties/PropertiesPage";
import SalesPage from "./pages/sales/Sales";
import Layout from "./components/layout";
import RentalPage from "./pages/rentals/RentalsPage";
import { useEffect } from "react";
function App() {
  function parseTokenFromUrl(tokenName,url) {
    const tokenMatch = url.match(new RegExp(`${tokenName}=([^&]+)`));
    return tokenMatch ? tokenMatch[1] : null;
  }
  useEffect(()=>{
    const url=window.location.href
    if(url.includes("id_token")){
      const idToken = parseTokenFromUrl('id_token',url);
      localStorage.setItem("Token",idToken);
    }
  },[])


  return (
    <Router>
      <Layout>
        <Routes>
          <Route path="/" element={<PropertiesPage />} />
          <Route path="/sales" element={<SalesPage />} />
          <Route path="/rentals" element={<RentalPage />} />
        </Routes>
      </Layout>
    </Router>
  );
}

export default App;
