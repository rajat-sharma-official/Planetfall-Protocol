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
Danriel: This isn’t scrap. It’s a <b>relic</b>—context matters.
Danriel: If you pull it apart, you destroy the only honest thing it has left: its history.

-> lvl_0_menu

=== lvl_0_menu ===
+ [Leave.] -> END
+ [I need that scrap for repairs.] -> push_0
+ [Why do you care so much about one relic?] -> why_0
+ [Can I at least look at it?] -> look_0

=== hub_lvl1 ===
Danriel: I said step back.
Danriel: You don’t get to rip history apart because you’re impatient.
Danriel: This isn’t your crash site to strip clean.

-> lvl_1_menu

=== lvl_1_menu ===
+ [Leave.] -> END
+ [I’m taking it anyway.] -> push_1
+ [You scholars don’t own the whole world.] -> why_1
+ [Fine. Explain what it is.] -> look_1

=== hub_lvl2 ===
Danriel: Enough.
Danriel: All salvage in <b>Virelia Frontier</b> belongs to the scholars for study.
Danriel: Touch it again and you’ll have a problem with more than my patience.

-> lvl_2_menu

=== lvl_2_menu ===
+ [Leave.] -> END
+ [STEAL] -> steal_scrap
+ [This is theft dressed as “research.”] -> why_2
+ [Tell Sanya I’m done playing nice.] -> look_2


=== push_0 ===
Danriel: No.
Danriel: Back away.
Danriel: I understand desperation. I do not excuse it.

~ danriel_block_level += 1
{ danriel_block_level > 2:
    ~ danriel_block_level = 2
}
-> lvl_0_menu

=== why_0 ===
Danriel: Because once it’s scattered, it’s gone.
Danriel: You can’t rebuild what you don’t understand.
Danriel: And you can’t understand what you’ve reduced to pocket-sized pieces.

~ danriel_block_level += 1
{ danriel_block_level > 2:
    ~ danriel_block_level = 2
}
-> lvl_0_menu

=== look_0 ===
Danriel: Fine.
Danriel: Look with your eyes. Not with your hands.
Danriel: See the <b>markings</b>? The wear pattern? The way the alloy holds its edge?
Danriel: That’s a <b>story</b>. It deserves more than greed.
-> lvl_0_menu

=== push_1 ===
Danriel: No.
Danriel: Back away.
Danriel: If you take it, you’re not “surviving.” You’re stealing.

~ danriel_block_level += 1
{ danriel_block_level > 2:
    ~ danriel_block_level = 2
}
-> lvl_1_menu

=== why_1 ===
Danriel: Because once it’s scattered, it’s gone.
Danriel: And you can’t rebuild what you don’t understand.
Danriel: If you think we’re hoarding, you don’t understand what’s at stake.

~ danriel_block_level += 1
{ danriel_block_level > 2:
    ~ danriel_block_level = 2
}
-> lvl_1_menu

=== look_1 ===
Danriel: This plate isn’t just “metal.”
Danriel: It’s a component of a system that outlived its makers.
Danriel: The scholars don’t study relics for fun.
Danriel: We study them because the future depends on what we learn.
-> lvl_1_menu

=== why_2 ===
Danriel: Call it what you want.
Danriel: Without this, we stay in the dark forever.
Danriel: If you want a future, stop trying to buy it with theft.

~ danriel_block_level += 1
{ danriel_block_level > 2:
    ~ danriel_block_level = 2
}
-> lvl_2_menu

=== look_2 ===
Danriel: Go ahead.
Danriel: Tell Sanya you chose pride over patience.
Danriel: She’ll still smile. She always does.
-> lvl_2_menu

=== steal_scrap ===
{ stolen_scrap:
    You already took scrap from here.

- else:
    You snatch the fragment the moment his eyes shift.
    It feels heavier than it should—like you stole more than metal.
    ~ stolen_scrap = true
    ~ Dialogue_scrap += 1
    ~ giveScrap(1)
}
-> END