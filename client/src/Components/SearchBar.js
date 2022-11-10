import Paper from "@mui/material/Paper";
import InputBase from "@mui/material/InputBase";
import Divider from "@mui/material/Divider";
import IconButton from "@mui/material/IconButton";
import SearchIcon from "@mui/icons-material/Search";

import { useState } from 'react';
import { useNavigate } from "react-router-dom";

export default function SearchBar() {

    let navigate = useNavigate();
    const [searchInput, setSearchInput] = useState("");

    const search = () => {
        navigate(`/Search/${searchInput}`);
    }

    return (
        <Paper
        component="form"
        sx={{ p: "2px 4px", display: "flex", alignItems: "center", width: 400 }}
        >
        <InputBase
            sx={{ ml: 1, flex: 1 }}
            placeholder="Search For Users"
            inputProps={{ "aria-label": "search google maps" }}
            value={searchInput}
            onChange={(e) => setSearchInput(e.target.value)}
        />
        <Divider sx={{ height: 28, m: 0.5 }} orientation="vertical" />
        <IconButton type="button" sx={{ p: "10px" }} aria-label="search" onClick={search}>
            <SearchIcon />
        </IconButton>
        </Paper>
    );
}
