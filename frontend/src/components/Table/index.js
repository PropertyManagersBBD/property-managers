import { useEffect, useState } from "react";
import "./style.css";
import { useLocation } from "react-router-dom";
import SearchOutlinedIcon from "@mui/icons-material/SearchOutlined";
import ArrowForwardIcon from "@mui/icons-material/ArrowForward";
import ArrowBackIcon from "@mui/icons-material/ArrowBack";
import FilterListOutlinedIcon from "@mui/icons-material/FilterListOutlined";
function ContentTable() {
  const [pageLocation, setLocation] = useState("/");
  const location = useLocation();
  const [data, setData]=useState([])
  useEffect(() => {
    setLocation(location.pathname);


    if(location.pathname==="/sales"){

      setData([
        { contractId: 1, propertyId: 8, sellerId: 2, buyerId: 9, capacity: 2, price: "Ð 12.59" },
        { contractId: 2, propertyId: 12, sellerId: 3, buyerId: 10, capacity: 3, price: "Ð 25.00" },
        { contractId: 3, propertyId: 5, sellerId: 4, buyerId: 11, capacity: 1, price: "Ð 15.75" },
        { contractId: 4, propertyId: 9, sellerId: 5, buyerId: 12, capacity: 4, price: "Ð 8.20" },
        { contractId: 5, propertyId: 15, sellerId: 6, buyerId: 13, capacity: 2, price: "Ð 19.99" },
        { contractId: 6, propertyId: 7, sellerId: 7, buyerId: 14, capacity: 5, price: "Ð 7.65" },
        { contractId: 7, propertyId: 14, sellerId: 8, buyerId: 15, capacity: 3, price: "Ð 22.10" },
        { contractId: 8, propertyId: 6, sellerId: 9, buyerId: 16, capacity: 1, price: "Ð 13.50" },
        { contractId: 9, propertyId: 3, sellerId: 10, buyerId: 17, capacity: 4, price: "Ð 18.30" },
        { contractId: 10, propertyId: 13, sellerId: 11, buyerId: 18, capacity: 2, price: "Ð 21.95" },
        { contractId: 11, propertyId: 10, sellerId: 12, buyerId: 19, capacity: 3, price: "Ð 11.80" },
        { contractId: 12, propertyId: 11, sellerId: 13, buyerId: 20, capacity: 5, price: "Ð 24.60" },
        { contractId: 13, propertyId: 4, sellerId: 14, buyerId: 21, capacity: 1, price: "Ð 14.25" },
        { contractId: 14, propertyId: 2, sellerId: 15, buyerId: 22, capacity: 2, price: "Ð 27.30" },
        { contractId: 15, propertyId: 1, sellerId: 16, buyerId: 23, capacity: 4, price: "Ð 10.00" },
        { contractId: 16, propertyId: 17, sellerId: 17, buyerId: 24, capacity: 3, price: "Ð 23.15" },
        { contractId: 17, propertyId: 18, sellerId: 18, buyerId: 25, capacity: 1, price: "Ð 16.50" },
        { contractId: 18, propertyId: 16, sellerId: 19, buyerId: 26, capacity: 2, price: "Ð 9.75" },
        { contractId: 19, propertyId: 20, sellerId: 20, buyerId: 27, capacity: 5, price: "Ð 26.80" },
        { contractId: 20, propertyId: 19, sellerId: 21, buyerId: 28, capacity: 3, price: "Ð 12.40" },
        { contractId: 21, propertyId: 21, sellerId: 22, buyerId: 29, capacity: 1, price: "Ð 22.75" },
        { contractId: 22, propertyId: 22, sellerId: 23, buyerId: 30, capacity: 4, price: "Ð 19.20" },
        { contractId: 23, propertyId: 23, sellerId: 24, buyerId: 31, capacity: 2, price: "Ð 11.50" },
        { contractId: 24, propertyId: 24, sellerId: 25, buyerId: 32, capacity: 3, price: "Ð 17.85" },
        { contractId: 25, propertyId: 25, sellerId: 26, buyerId: 33, capacity: 5, price: "Ð 20.60" },
      ]);
    }else if(location.pathname==="/"){
      setData( [
        { propertyId: 1, ownerId: 3, capacity: 4, listRent: false, listSale: false, pending: false },
        { propertyId: 2, ownerId: 1, capacity: 2, listRent: true, listSale: false, pending: false },
        { propertyId: 3, ownerId: 2, capacity: 5, listRent: false, listSale: true, pending: true },
        { propertyId: 4, ownerId: 4, capacity: 3, listRent: true, listSale: true, pending: false },
        { propertyId: 5, ownerId: 5, capacity: 6, listRent: false, listSale: false, pending: true },
        { propertyId: 6, ownerId: 1, capacity: 4, listRent: true, listSale: false, pending: false },
        { propertyId: 7, ownerId: 2, capacity: 3, listRent: false, listSale: true, pending: false },
        { propertyId: 8, ownerId: 3, capacity: 2, listRent: true, listSale: true, pending: true },
        { propertyId: 9, ownerId: 4, capacity: 5, listRent: false, listSale: false, pending: false },
        { propertyId: 10, ownerId: 5, capacity: 4, listRent: true, listSale: false, pending: true },
        { propertyId: 11, ownerId: 1, capacity: 3, listRent: false, listSale: true, pending: false },
        { propertyId: 12, ownerId: 2, capacity: 6, listRent: true, listSale: true, pending: false },
        { propertyId: 13, ownerId: 3, capacity: 5, listRent: false, listSale: false, pending: true },
        { propertyId: 14, ownerId: 4, capacity: 4, listRent: true, listSale: false, pending: false },
        { propertyId: 15, ownerId: 5, capacity: 2, listRent: false, listSale: true, pending: false },
        { propertyId: 16, ownerId: 1, capacity: 3, listRent: true, listSale: true, pending: true },
        { propertyId: 17, ownerId: 2, capacity: 5, listRent: false, listSale: false, pending: false },
        { propertyId: 18, ownerId: 3, capacity: 6, listRent: true, listSale: false, pending: true },
        { propertyId: 19, ownerId: 4, capacity: 3, listRent: false, listSale: true, pending: false },
        { propertyId: 20, ownerId: 5, capacity: 4, listRent: true, listSale: true, pending: false },
        { propertyId: 21, ownerId: 1, capacity: 5, listRent: false, listSale: false, pending: true },
        { propertyId: 22, ownerId: 2, capacity: 2, listRent: true, listSale: false, pending: false },
        { propertyId: 23, ownerId: 3, capacity: 3, listRent: false, listSale: true, pending: true },
        { propertyId: 24, ownerId: 4, capacity: 6, listRent: true, listSale: true, pending: false },
        { propertyId: 25, ownerId: 5, capacity: 4, listRent: false, listSale: false, pending: false },
      ])
    }else{
      setData([
        { contractId: 1, propertyId: 8, landlordId: 2, tenantId: 9, capacity: 2, price: "Ð 12.59" },
        { contractId: 2, propertyId: 12, landlordId: 3, tenantId: 10, capacity: 3, price: "Ð 25.00" },
        { contractId: 3, propertyId: 5, landlordId: 4, tenantId: 11, capacity: 1, price: "Ð 15.75" },
        { contractId: 4, propertyId: 9, landlordId: 5, tenantId: 12, capacity: 4, price: "Ð 8.20" },
        { contractId: 5, propertyId: 15, landlordId: 6, tenantId: 13, capacity: 2, price: "Ð 19.99" },
        { contractId: 6, propertyId: 7, landlordId: 7, tenantId: 14, capacity: 5, price: "Ð 7.65" },
        { contractId: 7, propertyId: 14, landlordId: 8, tenantId: 15, capacity: 3, price: "Ð 22.10" },
        { contractId: 8, propertyId: 6, landlordId: 9, tenantId: 16, capacity: 1, price: "Ð 13.50" },
        { contractId: 9, propertyId: 3, landlordId: 10, tenantId: 17, capacity: 4, price: "Ð 18.30" },
        { contractId: 10, propertyId: 13, landlordId: 11, tenantId: 18, capacity: 2, price: "Ð 21.95" },
        { contractId: 11, propertyId: 10, landlordId: 12, tenantId: 19, capacity: 3, price: "Ð 11.80" },
        { contractId: 12, propertyId: 11, landlordId: 13, tenantId: 20, capacity: 5, price: "Ð 24.60" },
        { contractId: 13, propertyId: 4, landlordId: 14, tenantId: 21, capacity: 1, price: "Ð 14.25" },
        { contractId: 14, propertyId: 2, landlordId: 15, tenantId: 22, capacity: 2, price: "Ð 27.30" },
        { contractId: 15, propertyId: 1, landlordId: 16, tenantId: 23, capacity: 4, price: "Ð 10.00" },
        { contractId: 16, propertyId: 17, landlordId: 17, tenantId: 24, capacity: 3, price: "Ð 23.15" },
        { contractId: 17, propertyId: 18, landlordId: 18, tenantId: 25, capacity: 1, price: "Ð 16.50" },
        { contractId: 18, propertyId: 16, landlordId: 19, tenantId: 26, capacity: 2, price: "Ð 9.75" },
        { contractId: 19, propertyId: 20, landlordId: 20, tenantId: 27, capacity: 5, price: "Ð 26.80" },
        { contractId: 20, propertyId: 19, landlordId: 21, tenantId: 28, capacity: 3, price: "Ð 12.40" },
        { contractId: 21, propertyId: 21, landlordId: 22, tenantId: 29, capacity: 1, price: "Ð 22.75" },
        { contractId: 22, propertyId: 22, landlordId: 23, tenantId: 30, capacity: 4, price: "Ð 19.20" },
        { contractId: 23, propertyId: 23, landlordId: 24, tenantId: 31, capacity: 2, price: "Ð 11.50" },
        { contractId: 24, propertyId: 24, landlordId: 25, tenantId: 32, capacity: 3, price: "Ð 17.85" },
        { contractId: 25, propertyId: 25, landlordId: 26, tenantId: 33, capacity: 5, price: "Ð 20.60" },
      ])
    }
  
  }, [location.pathname]);

  
  
  return (
    <article className="Wrapper">
      <section className="navButtons">
        <button>
          Next <ArrowForwardIcon />{" "}
        </button>
        <button>
          <ArrowBackIcon /> Back{" "}
        </button>
      </section>
      <section className="input-wrapper">
        <section className="SearchContainer">
          <input
            id="searchInput"
            type="text"
            placeholder="Search"
            className="searchField"
          ></input>
          <SearchOutlinedIcon />
        </section>

        <article className="selectWrapper">
          <select id="capacitySelect" name="capacity" className="selectField">
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
          <FilterListOutlinedIcon/>
        </article>

        <article className="selectWrapper">
          <select id="contractSelect" name="contract" className="selectField">
            <option value="All">All</option>
            <option value="Rental">Rental</option>
            <option value="Purchase">Purchase</option>
          </select>
          <FilterListOutlinedIcon/>
        </article>
      </section>

      <article className="main-table">
        {pageLocation === "/" && (
          <article className="tableBody">
            <section className="Header">
              <h2>Property</h2>
              <h2>Owner ID</h2>
              <h2>Capacity</h2>
              <h2>Listed for Rent</h2>
              <h2>Listed for Sale</h2>
              <h2>Pending</h2>
            </section>

            {data.map((item, index) => {
              return (
                <article className="Entry" key={index}>
                  <h2>{item.propertyId}</h2>
                  <h2>{item.ownerId}</h2>
                  <h2>{item.capacity}</h2>
                  <h2>{item.listRent ? "True" : "False"}</h2>
                  <h2>{item.listSale ? "True" : "False"}</h2>
                  <h2>{item.pending ? "True" : "False"}</h2>
                </article>
              );
            })}
          </article>
        )}



{pageLocation === "/sales" && (
          <article className="tableBody">
            <section className="Header">
              <h2>Contract ID</h2>
              <h2>Property ID</h2>
              <h2>Seller ID</h2>
              <h2>Buyer ID</h2>
              <h2>Capacity</h2>
              <h2>Price</h2>
            </section>

            {data.map((item, index) => {
              return (
                <article className="Entry" key={index}>
                  <h2>{item.contractId}</h2>
                  <h2>{item.propertyId}</h2>
                  <h2>{item.sellerId}</h2>
                  <h2>{item.buyerId }</h2>
                  <h2>{item.capacity}</h2>
                  <h2>{item.price }</h2>
                </article>
              );
            })}
          </article>
        )}


{pageLocation === "/rentals" && (
          <article className="tableBody">
            <section className="Header">
              <h2>Contract ID</h2>
              <h2>Property ID</h2>
              <h2>Landlord ID</h2>
              <h2>Tenant ID</h2>
              <h2>Capacity</h2>
              <h2>Price</h2>
            </section>

            {data.map((item, index) => {
              return (
                <article className="Entry" key={index}>
                  <h2>{item.contractId}</h2>
                  <h2>{item.propertyId}</h2>
                  <h2>{item.landlordId}</h2>
                  <h2>{item.tenantId }</h2>
                  <h2>{item.capacity}</h2>
                  <h2>{item.price }</h2>
                </article>
              );
            })}
          </article>
        )}
      </article>
    </article>
  );
}

export default ContentTable;
