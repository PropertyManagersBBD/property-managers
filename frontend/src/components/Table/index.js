import { useEffect, useState } from "react";
import "./style.css";
import { useLocation } from "react-router-dom";
import SearchOutlinedIcon from '@mui/icons-material/SearchOutlined';
function ContentTable() {

    const [pageLocation, setLocation]=useState("/");
    const location=useLocation();
    useEffect(()=>{
        setLocation(location.pathname);
    },[location.pathname])
  return (
    <article className="Wrapper">
        <section className="input-wrapper">


      <input id="searchInput" type="text" placeholder="Search" className="inputField"></input> 
        
      {/* <label for="capacitySelect">Capacity:</label> */}
      <select id="capacitySelect" name="capacity" className="inputField">
      <option value="All">All</option>
        <option value="1">1</option>
        <option value="2">2</option>
        <option value="3">3</option>
        <option value="4">4</option>
        <option value="5">5</option>
        <option value="6">6</option>
        <option value="7">7</option>
        <option value="8">8</option>
      </select>

      {/* <label for="contractSelect">Contract Types:</label> */}
      <select id="contractSelect" name="contract" className="inputField">
      <option value="All">All</option>
        <option value="Rental">Rental</option>
        <option value="Purchase">Purchase</option>
      </select>
        </section>


        <article className="main-table">
            {
                pageLocation==="/" &&(
                    <section className="Header">

                    <h2>Property</h2>
                    <h2>OwnerID</h2>
                    <h2>Capacity</h2>
                    <h2>Listed for Rent</h2>
                    <h2>Listed for Sale</h2>
                    <h2>Pending</h2>
                </section>
                )
            }
           
        </article>
    </article>
  );
}

export default ContentTable;
