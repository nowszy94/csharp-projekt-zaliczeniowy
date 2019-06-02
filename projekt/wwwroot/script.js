const githubReposElement = $("#github-repos");
const savedReposElement = $("#saved-repos");

let githubRepos = [];
let savedRepos = [];
let username = "nowszy94";

const rerender = function () {
    renderGithubRepos();
    renderSavedRepos();
}

const renderGithubRepos = function () {
    githubReposElement.empty();
    if (githubRepos.length === 0) {
        githubReposElement.append(noRepos);
    } else {
        for (i = 0; i < githubRepos.length; i++) {
            const repo = githubRepos[i];
            const itemToAdd = $(githubRepoItem(repo));
            itemToAdd.click(function () { addRepo(repo) })
            githubReposElement.append(itemToAdd);
        }
    }
}

const renderSavedRepos = function () {
    savedReposElement.empty();
    if (savedRepos.length === 0) {
        savedReposElement.append(noRepos);
    } else {
        for (i = 0; i < savedRepos.length; i++) {
            const repo = savedRepos[i];
            const itemToAdd = $(savedRepoItem(repo));
            itemToAdd.click(function () { deleteSavedRepo(repo) })
            savedReposElement.append(itemToAdd);
        }
    }
}

const githubRepoItem = ({ name, html_name, forks_url, forks, watchers }, onAdd) => {
    return `
    <div class="repo">
        <h2><a href="${html_name}">${name}</a></h2>
        <p>Forks: ${forks}</p> <a class="fork-me" href="${forks_url}">Fork</a>
        <p>Watchers: ${watchers}</p>
        <button>Dodaj</button>
    </div>`
};


const savedRepoItem = ({ name, html_name }, onRemove) => {
    return `
        <div class="repo saved">
            <h2><a href="${html_name}">${name}</a></h2>
            <button onclick="${onRemove}">Usun</button>
        </div>
    `
};

const noRepos = `<p>Brak repozytoriow</p>`;

const getRepos = function (user = username) {
    $.get("https://localhost:5001/api/github?user=" + user, (data) => {
        githubRepos = data.reposFromGithub;
        savedRepos = data.savedRepos;
        rerender();
    })
}

const addRepo = function(repo) {
    $.ajax({
        url: "https://localhost:5001/api/github",
        contentType: "application/json; charset=utf-8",
        method: 'POST',
        dataType: 'json',
        data: JSON.stringify(repo)
    }).always(() => getRepos());
};

const deleteSavedRepo = function (repo) {
    $.ajax({
        url: "https://localhost:5001/api/github/" + repo.id,
        method: 'DELETE'
    }).always(() => getRepos())
}

const initSearchBar = function () {
    const input = $("#search-username");
    const button = $("#search-button");

    button.click(function () {
        username = input.val();
        getRepos(username);
    })
}

initSearchBar();
getRepos();