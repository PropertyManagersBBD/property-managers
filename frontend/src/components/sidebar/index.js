import "./style.css";
import { ReactComponent as Logo } from "../../media/logo.svg";
import MapsHomeWorkOutlinedIcon from "@mui/icons-material/MapsHomeWorkOutlined";
import FolderOutlinedIcon from "@mui/icons-material/FolderOutlined";
import Inventory2OutlinedIcon from "@mui/icons-material/Inventory2Outlined";
import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";

function SideBar({ Selected }) {
  console.log(Selected);
  const [username, setUsername] = useState("@user-name");
  const [selectedButton, setSelectedButton] = useState(Selected);
  console.log(selectedButton);
  const navigate = useNavigate();

  useEffect(() => {
    setSelectedButton(Selected);
  }, [Selected]);
  return (
    <article className="fullSideBar">
      <article className="logoWrapper">
        <Logo className="logo" />
      </article>
      <button className="loginButton">Login</button>
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
