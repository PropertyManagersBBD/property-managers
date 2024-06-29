import "./style.css";
import { ReactComponent as Logo } from "../../media/logo.svg";
import MapsHomeWorkOutlinedIcon from "@mui/icons-material/MapsHomeWorkOutlined";
import FolderOutlinedIcon from "@mui/icons-material/FolderOutlined";
import Inventory2OutlinedIcon from "@mui/icons-material/Inventory2Outlined";
import { useState } from "react";

function SideBar({Selected}) {
  const [username, setUsername] = useState("@user-name");

  const [selectedButton,setSelectedButton]=useState(Selected);
  console.log(selectedButton===0? "selectedButton":"unselectedButton")
  return (
    <article className="fullSideBar">
      <article className="logoWrapper">
        <Logo className="logo" />
      </article>
      <button className="loginButton">Login</button>
      <section className="Location-Buttons">
        <button className={selectedButton===0? "selectedButton":"unselectedButton"}>
          <Inventory2OutlinedIcon /> Properties
        </button>
        <button className={selectedButton===1? "selectedButton":"unselectedButton"}>
          <MapsHomeWorkOutlinedIcon /> Sale contracts
        </button>
        <button className={selectedButton===2? "selectedButton":"unselectedButton"}>
          <FolderOutlinedIcon /> Rental contracts
        </button>
      </section>

      <section>
        <h2>{username}</h2>
      </section>
    </article>
  );
}

export default SideBar;
