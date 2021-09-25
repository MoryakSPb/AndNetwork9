﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using AndNetwork9.Shared;
using AndNetwork9.Shared.Elections;
using AndNetwork9.Shared.Enums;
using Microsoft.AspNetCore.Components;

namespace AndNetwork9.Client.Pages.Elections
{
    public partial class ElectionVoting
    {
        [Inject]
        public HttpClient Client { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }
        [Parameter]
        public CouncilElection CurrentElections { get; set; }
        [Parameter]
        public IReadOnlyCollection<Member> AllMembers { get; set; }
        public Dictionary<Direction, Dictionary<int, uint>> Bulletins { get; set; } =
            new(Enum.GetValues<Direction>().Count(x => x > Direction.None));
        public bool Initialized { get; set; } = false;

        public Dictionary<Direction, bool> SendsAllowed { get; set; } =
            new(Enum.GetValues<Direction>().Count(x => x > Direction.None));
        public bool SendAllowed => SendsAllowed.All(x => x.Value);

        protected override async void OnInitialized()
        {
            CurrentElections ??= await Client.GetFromJsonAsync<CouncilElection>("api/Election/current");
            AllMembers ??= await Client.GetFromJsonAsync<Member[]>("api/member/all");
            foreach (var vote in CurrentElections!.Votes)
            {
                Bulletins.Add(vote.Direction, vote.Votes.ToDictionary(x => x.Key, _ => 0U));
                SendsAllowed.Add(vote.Direction, !vote.VoteAllowed || vote.Votes.Count == 1);
            }

            Initialized = true;
            StateHasChanged();
        }

        private async System.Threading.Tasks.Task Send()
        {
            await Client.PostAsJsonAsync("api/Election/vote",
                Bulletins.Where(x => SendsAllowed[x.Key]).ToDictionary(x => (int)x.Key, x => x.Value));
            NavigationManager.NavigateTo(NavigationManager.BaseUri);
        }

        private void UpdateSendAllowed(Direction direction, bool allow)
        {
            SendsAllowed[direction] = allow;
            StateHasChanged();
        }
    }
}