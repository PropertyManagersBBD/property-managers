import "./style.css";
import { ReactComponent as Logo } from "../../media/logo.svg";
import MapsHomeWorkOutlinedIcon from "@mui/icons-material/MapsHomeWorkOutlined";
import FolderOutlinedIcon from "@mui/icons-material/FolderOutlined";
import Inventory2OutlinedIcon from "@mui/icons-material/Inventory2Outlined";
import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";

function SideBar({ Selected }) {
  const [username, setUsername] = useState("@user-name");
  const [selectedButton, setSelectedButton] = useState(Selected);
  const navigate = useNavigate();

  useEffect(() => {
    setSelectedButton(Selected);
  }, [Selected]);
  return (
    <article className="fullSideBar">
      <article className="logoWrapper">
        <Logo className="logo" />
      </article>
      <button className="loginButton" onClick={()=>{
        window.location.href="https://dev-property-manager.auth.eu-west-1.amazoncognito.com/login?client_id=7tkql2hk58h484i70ji3n9hvb6&response_type=token&scope=email+openid+phone&redirect_uri=http%3A%2F%2Flocalhost%3A3000";
      }}>Login</button>
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

      <footer className="Footer">
        <h2>{username}</h2>
      </footer>
    </article>
  );
}

export default SideBar;
