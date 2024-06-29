import './App.css';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import PropertiesPage from './pages/properties/PropertiesPage';
import Layout from './components/layout';
function App() {
  return (
    <Router>
    <Layout>
      <Routes>
      <Route path="/" element={<PropertiesPage />} />

        {/* <Route path="/about" component={AboutPage} /> */}
        {/* Add more routes here */}
      </Routes>
    </Layout>
  </Router>
  );
}

export default App;
