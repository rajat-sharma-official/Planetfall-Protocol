INCLUDE globals.ink
-> start

=== start ===
~ register_npc(NPC.danriel)

{ danriel_block_level >= 2:
    -> hub_lvl2
- else:
    { danriel_block_level == 1:
        -> hub_lvl1
    - else:
        -> hub_lvl0
    }
}

=== hub_lvl0 ===
Danriel: Step back, please.
Danriel: This isn’t scrap. It’s a relic—context matters.
-> lvl_0_menu

=== lvl_0_menu ===
+ [Leave.] -> END
+ [I need that scrap for repairs.] -> push_0
+ [Why do you care so much about one relic?] -> why_0
+ [Can I at least look at it?] -> look_0

=== hub_lvl1 ===
Danriel: I said step back.
Danriel: You don’t get to rip history apart because you’re impatient.
-> lvl_1_menu

=== lvl_1_menu ===
+ [Leave.] -> END
+ [I’m taking it anyway.] -> push_1
+ [You scholars don’t own the whole world.] -> why_1
+ [Fine. Explain what it is.] -> look_1

=== hub_lvl2 ===
Danriel: Enough.
Danriel: All salvage in Virelia Frontier belongs to the scholars for study.
Danriel: Touch it again and you’ll have a problem with more than my patience.
-> lvl_2_menu

=== lvl_2_menu ===
+ [STEAL] -> steal_scrap
+ [I need it more than you do.] -> push_2
+ [This is theft dressed as “research.”] -> why_2
+ [Tell Sanya I’m done playing nice.] -> look_2


=== push_0 ===
Danriel: No.
Danriel: Back away.

~ danriel_block_level += 1
{ danriel_block_level > 2:
    ~ danriel_block_level = 2
}
-> lvl_0_menu

=== why_0 ===
Danriel: Because once it’s scattered, it’s gone.
~ danriel_block_level += 1
{ danriel_block_level > 2:
    ~ danriel_block_level = 2
}
-> lvl_0_menu

=== look_0 ===
Danriel: fine...

-> lvl_0_menu

=== push_1 ===
Danriel: No.
Danriel: Back away.

~ danriel_block_level += 1
{ danriel_block_level > 2:
    ~ danriel_block_level = 2
}
-> lvl_1_menu

=== why_1 ===
Danriel: Because once it’s scattered, it’s gone.
Danriel: And you can’t rebuild what you don’t understand.
~ danriel_block_level += 1
{ danriel_block_level > 2:
    ~ danriel_block_level = 2
}
-> lvl_1_menu

=== look_1 ===
Danriel: Look with your eyes, then.
Danriel: Not with your hands.
-> lvl_1_menu

=== push_2 ===
Danriel: No.
Danriel: Back away.

~ danriel_block_level += 1
{ danriel_block_level > 2:
    ~ danriel_block_level = 2
}
-> lvl_2_menu

=== why_2 ===
Danriel: Because once it’s scattered, it’s gone.
Danriel: And you can’t rebuild what you don’t understand.
~ danriel_block_level += 1
{ danriel_block_level > 2:
    ~ danriel_block_level = 2
}
-> lvl_2_menu

=== look_2 ===
Danriel: Look with your eyes, then.
Danriel: Not with your hands.
-> lvl_2_menu

=== steal_scrap ===
{ stolen_scrap:
    You took scrap Already 

- else:
    Scrap taken
    ~ stolen_scrap = true
    ~ Dialogue_scrap += 1
    ~ giveScrap(1)
    
}
->END

