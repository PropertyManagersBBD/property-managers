import "./style.css";
import { ReactComponent as Logo } from "../../media/logo.svg";
import MapsHomeWorkOutlinedIcon from "@mui/icons-material/MapsHomeWorkOutlined";
import FolderOutlinedIcon from "@mui/icons-material/FolderOutlined";
import Inventory2OutlinedIcon from "@mui/icons-material/Inventory2Outlined";
import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { getUsername } from "../../shared/handleJwt";
import MenuIcon from '@mui/icons-material/Menu';

function SideBar({ Selected }) {
  const [username, setUsername] = useState("@user-name");
  const [selectedButton, setSelectedButton] = useState(Selected);
  const [loggedIn, setLoggedIn] = useState(false);
  const [showMenu,setShowMenu]=useState(false)
  const navigate = useNavigate();

  useEffect(() => {
    setSelectedButton(Selected);
  }, [Selected]);
  

  useEffect(() => {
    const token = localStorage.getItem("Token");
    if (token) {
      setLoggedIn(true);
    } else {
      setLoggedIn(false);
    }
  }, []);

  useEffect(()=>{
    if(loggedIn){
      setUsername(getUsername());
    }else{
      setUsername("Welcome User")
    }
  },[loggedIn])
  return (
    <article className="fullSideBar">
      <article className="logoWrapper">
        <Logo className="logo" />
      </article>
      
      <section className="Location-Buttons">
        <button
          className={
            selectedButton === "/" ? "selectedButton" : "unselectedButton"
          }
          onClick={() => {
            if (selectedButton !== "/") {
              navigate("/");
            }
          }}
        >
          <Inventory2OutlinedIcon /> Properties
        </button>
        <button
          className={
            selectedButton === "/sales" ? "selectedButton" : "unselectedButton"
          }
          onClick={() => {
            if (selectedButton !== "/sales") {
              navigate("/sales");
            }
          }}
        >
          <MapsHomeWorkOutlinedIcon /> Sale contracts
        </button>
        <button
          className={
            selectedButton === "/rentals"
              ? "selectedButton"
              : "unselectedButton"
          }
          onClick={() => {
            if (selectedButton !== "/rentals") {
              navigate("/rentals");
            }
          }}
        >
          <FolderOutlinedIcon /> Rental contracts
        </button>
      </section>

      <section className="Footer">
        <h2>{username}</h2>
      </section>
      {!loggedIn ? (
        <button
          className="loginButton"
          onClick={() => {
            window.location.href =
              "https://dev-property-manager.auth.eu-west-1.amazoncognito.com/login?client_id=7tkql2hk58h484i70ji3n9hvb6&response_type=token&scope=email+openid+phone&redirect_uri=http%3A%2F%2Flocalhost%3A3000";
          }}
        >
          Login
        </button>
      ) : (
        <button
          className="loginButton"
          onClick={() => {
            setLoggedIn(false);
            localStorage.removeItem("Token");
          }}
        >
          Logout
        </button>
      )}

      <button className="Menu" onClick={()=>{
        setShowMenu(!showMenu)
      }}><MenuIcon className="menu-icon"/></button>


      {showMenu &&(
        <section className="full-menu">
{!loggedIn ? (
        <button
          className="loginButton-menu"
          onClick={() => {
            window.location.href =
              "https://dev-property-manager.auth.eu-west-1.amazoncognito.com/login?client_id=7tkql2hk58h484i70ji3n9hvb6&response_type=token&scope=email+openid+phone&redirect_uri=http%3A%2F%2Flocalhost%3A3000";
          }}
        >
          Login
        </button>
      ) : (
        <button
          className="loginButton-menu"
          onClick={() => {
            setLoggedIn(false);
            localStorage.removeItem("Token");
          }}
        >
          Logout
        </button>
      )}
          <section className="Location-Buttons-menu">
        <button
          className={
            selectedButton === "/" ? "selectedButton" : "unselectedButton"
          }
          onClick={() => {
            if (selectedButton !== "/") {
              navigate("/");
            }
          }}
        >
           Properties
        </button>
        <button
          className={
            selectedButton === "/sales" ? "selectedButton" : "unselectedButton"
          }
          onClick={() => {
            if (selectedButton !== "/sales") {
              navigate("/sales");
            }
          }}
        >
          Sale contracts
        </button>
        <button
          className={
            selectedButton === "/rentals"
              ? "selectedButton"
              : "unselectedButton"
          }
          onClick={() => {
            if (selectedButton !== "/rentals") {
              navigate("/rentals");
            }
          }}
        >
          Rental contracts
        </button>



        
      </section>
        </section>
      )}

    
    </article>
  );
}

export default SideBar;
