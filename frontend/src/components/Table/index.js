import { useEffect, useState } from "react";
import "./style.css";
import { useLocation } from "react-router-dom";
import SearchOutlinedIcon from "@mui/icons-material/SearchOutlined";
import ArrowForwardIcon from "@mui/icons-material/ArrowForward";
import ArrowBackIcon from "@mui/icons-material/ArrowBack";
import FilterListOutlinedIcon from "@mui/icons-material/FilterListOutlined";
import {
  getSalesContracts,
  getProperties,
  getRentalContracts,
} from "../../shared/requests";
function ContentTable() {
  const [pageLocation, setLocation] = useState("/");
  const location = useLocation();
  const [data, setData] = useState([]);
  const [pageNumber, setPageNumber] = useState(1);
  const [pageSize, setPageSize] = useState(7);
  const [capacity, setCapacity] = useState();
  const [propertyId, setPropertyId] = useState();
  const [ownerId, setOwnerId] = useState();
  useEffect(() => {
    setLocation(location.pathname);

    if (location.pathname === "/") {
      getProperties(pageNumber, pageSize, propertyId, ownerId, capacity).then(
        (returnData) => {
          setData(returnData);
        }
      );
    } else if (location.pathname === "/sales") {
      getSalesContracts(
        pageNumber,
        pageSize,
        propertyId,
        ownerId,
        capacity
      ).then((returnData) => {
        setData(returnData);
      });
    } else {
      getRentalContracts(
        pageNumber,
        pageSize,
        ownerId,
        propertyId,
        capacity
      ).then((returnData) => {
        setData(returnData);
      });
    }
  }, [location.pathname, pageNumber, propertyId, capacity, ownerId]);

  const handleCapacityChange = (event) => {
    if (event.target.value === "All") {
      setCapacity(null);
    } else {
      setCapacity(event.target.value);
    }
  };

  const handlePropertySearchChange = (event) => {
    const target = event.target.value;
    if (/^\d+$/.test(target)) {
      setPropertyId(target);
    } else if (target === "") {
      setPropertyId(null);
    }
  };

  const handleOwnerSearchChange = (event) => {
    const target = event.target.value;
    if (/^\d+$/.test(target)) {
      setOwnerId(target);
    } else if (target === "") {
      setOwnerId(null);
    }
  };
  return (
    <article className="Wrapper">
      
      <section className="input-wrapper">
        <section className="SearchContainer">
          <input
            id="PropertySearchInput"
            type="text"
            placeholder="Search Property ID"
            className="searchField"
            onChange={handlePropertySearchChange}
          ></input>
          <SearchOutlinedIcon />
        </section>

        <article className="selectWrapper">
          <select
            id="capacitySelect"
            name="capacity"
            className="selectField"
            onChange={handleCapacityChange}
          >
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
          <FilterListOutlinedIcon />
        </article>

        {pageLocation === "/" && (
          <section className="SearchContainer">
            <input
              id="OwnerIDSearchInput"
              type="text"
              placeholder="Search Owner ID"
              className="searchField"
              onChange={handleOwnerSearchChange}
            ></input>
            <SearchOutlinedIcon />
          </section>
        )}

        {pageLocation === "/sales" && (
          <section className="SearchContainer">
            <input
              id="SellerIDSearchInput"
              type="text"
              placeholder="Search Seller ID"
              className="searchField"
              onChange={handleOwnerSearchChange}
            ></input>
            <SearchOutlinedIcon />
          </section>
        )}

        {pageLocation === "/rentals" && (
          <section className="SearchContainer">
            <input
              id="landLordSearchInput"
              type="text"
              placeholder="Search Landlord ID"
              className="searchField"
              onChange={handlePropertySearchChange}
            ></input>
            <SearchOutlinedIcon />
          </section>
        )}
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

          <section className="Content">

            {data ? (
              data.map((item, index) => {
                return (
                  <article className="Entry" key={index}>
                    <h2>{item.id}</h2>
                    <h2>{item.ownerId}</h2>
                    <h2>{item.capacity}</h2>
                    <h2>{item.listedForRent ? "True" : "False"}</h2>
                    <h2>{item.listedForSale ? "True" : "False"}</h2>
                    <h2>{item.pending ? "True" : "False"}</h2>
                  </article>
                );
              })
            ) : (
              <article className="Error-Message">
                <h2>There Seems To Be A Problem</h2>
              </article>
            )}
          </section>
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

            {data ? (
              data.map((item, index) => (
                <article className="Entry" key={index}>
                  <h2>{item.id}</h2>
                  <h2>{item.propertyId}</h2>
                  <h2>{item.sellerId}</h2>
                  <h2>{item.buyerId}</h2>
                  <h2>{item.capacity}</h2>
                  <h2>Ð {item.price}</h2>
                </article>
              ))
            ) : (
              <article className="Error-Message">
                <h2>There Seems To Be A Problem</h2>
              </article>
            )}
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

            {data ? (
              data.map((item, index) => {
                return (
                  <article className="Entry" key={index}>
                    <h2>{item.id}</h2>
                    <h2>{item.propertyId}</h2>
                    <h2>{item.landlordId}</h2>
                    <h2>{item.tenantId}</h2>
                    <h2>{item.capacity}</h2>
                    <h2>Ð {item.price}</h2>
                  </article>
                );
              })
            ) : (
              <article className="Error-Message">
                <h2>There Seems To Be A Problem</h2>
              </article>
            )}
          </article>
        )}
      </article>
      <section className="navButtons">
        <button
          onClick={() => {
            if (pageNumber > 1) {
              setPageNumber(pageNumber - 1);
            }
          }}
        >
          <ArrowBackIcon />
        </button>
        <p>{pageNumber}</p>
        <button
          onClick={() => {
            if (data.length === 7) {
              setPageNumber(pageNumber + 1);
            }
          }}
        >
          <ArrowForwardIcon />
        </button>
      </section>
    </article>
  );
}

export default ContentTable;
