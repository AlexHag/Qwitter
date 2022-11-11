import { useParams, useNavigate } from "react-router-dom";
import { useState, useEffect } from 'react';

import AccountCircleIcon from '@mui/icons-material/AccountCircle';
import '../css/search.css';

function SearchFeedAll(props) {

    //let { username } = useParams();
    let navigate = useNavigate();

    const [searchResult, setSearchresult] = useState([]);

    useEffect(() => {
        fetch(`http://localhost:5295/SearchProfiles/`)
        .then(p => p.json())
        .then(p => setSearchresult(p));
    }, []);

    const goToProfile = (username) => {
        console.log(username);
        navigate(`/Profile/${username}`);
    };

    const renderSearchResult = () => {
        if(searchResult.status === 404) return <h2>No users found</h2>
        console.log(searchResult);
        return (
            <div className="search-container">
                <h1>Found {searchResult.length} users</h1>
                {searchResult.map(p => 
                    <div className={`search-result premium-${p.isPremium}`} onClick={() => goToProfile(p.username)}>
                        <AccountCircleIcon />
                        <h2 style={{marginLeft: "10px"}}>{p.username}</h2>
                    </div>
                )}
            </div>
        )
    };

    return (
        <>
            {renderSearchResult()}
        </>
    )
}

export default SearchFeedAll;