import "./App.css";
import { BrowserRouter as Router, Route, Routes, useLocation } from "react-router-dom";
import PropertiesPage from "./pages/properties/PropertiesPage";
import SalesPage from "./pages/sales/Sales";
import Layout from "./components/layout";
import RentalPage from "./pages/rentals/RentalsPage";
import { useEffect } from "react";
function App() {
  useEffect(()=>{
    const url=window.location.href
    console.log(url)
  })


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
